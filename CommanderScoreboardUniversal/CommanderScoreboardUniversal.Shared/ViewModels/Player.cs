using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommanderScoreboardUniversal.ViewModels
{
    [DataContract(IsReference = true)]
    public class Player : INotifyPropertyChanged
    {
        public Player() { if (CommanderDamage == null) CommanderDamage = new ObservableCollection<CommanderDamageItem>(); } //For serialization

        public Player(bool isInCommanderGame = true)
        {
            Life = isInCommanderGame ? 40 : 20;
            IsCommanderGame = isInCommanderGame;
            CommanderDamage = new ObservableCollection<CommanderDamageItem>();
        }

        DateTime lastTimeWeChangedLifeDelta = DateTime.Now;

        private void UpdateLife()
        {
            if (!(lastTimeWeChangedLifeDelta.Add(TimeSpan.FromSeconds(3)) > DateTime.Now)) // If more than 3 seconds ago.
            {
                Life += _lifedelta;
                _lifedelta = 0;
                Refresh();
            }
            else
            {
                Action temp = async () => { await Task.Delay(3000); UpdateLife(); };
                temp.Invoke();
            }
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Life { get; set; }
        public string LifeText { get { return string.Format("❤ {0}{1}", Life, LifeDelta == 0 ? "" : (LifeDelta > 0 ? "+" + LifeDelta : LifeDelta.ToString())); } }
        [DataMember]
        public int Poison { get; set; }
        public bool HasPoison { get { return Poison != 0; } }
        public string PoisonText { get { return string.Format("☠ {0}", Poison); } }

        /// <summary>
        /// I say probably because weird things happen in magic
        /// </summary>
        public bool IsProbablyDead { get { return Life < 1 || Poison >= 10 || CommanderDamage != null && CommanderDamage.Any(cdi => cdi.Amount >= 21); } }
        private int recastCost;

        [DataMember]
        public int CommanderAdditionalCost
        {
            get { return recastCost; }
            set { recastCost = value; Refresh(); }
        }

        public string CommanderText { get { return string.Format("ⓧ {0}", CommanderAdditionalCost); } }


        public bool HasCastedCommander
        {
            get { return CommanderAdditionalCost != 0; }
        }

        [DataMember]
        public ObservableCollection<CommanderDamageItem> CommanderDamage { get; set; }

        public void Refresh()
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public bool IsCommanderGame { get; set; }

        private int _lifedelta;
        public int LifeDelta { get { return _lifedelta; } set { _lifedelta = value; lastTimeWeChangedLifeDelta = DateTime.Now; Refresh(); UpdateLife(); } }
    }
}

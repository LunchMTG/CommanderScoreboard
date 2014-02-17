using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MTGLib
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

        private void UpdateLife()
        {
            Life += _lifedelta;
            _lifedelta = 0;
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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public bool IsCommanderGame { get; set; }//{ get { return CommanderAdditionalCost != 0 || CommanderDamage.Count != 0; } }


        private int _lifedelta;
        public int LifeDelta { get { return _lifedelta; } set { _lifedelta = value; Refresh(); UpdateLife(); Refresh(); } }
    }
}

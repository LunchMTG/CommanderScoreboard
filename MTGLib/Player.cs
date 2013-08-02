using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MTGLib
{
    public class Player : INotifyPropertyChanged
    {
        public Player() { } //For serialization

        public Player(bool isInCommanderGame = true)
        {
            Life = isInCommanderGame ? 40 : 20;
            CommanderDamage = new ObservableCollection<CommanderDamageItem>();
        }

        public string Name { get; set; }
        public int Life { get; set; }
        public string LifeText { get { return string.Format("❤ {0}", Life); } }
        public int Poison { get; set; }
        public bool HasPoison { get { return Poison != 0; } }
        public string PoisonText { get { return string.Format("☠ {0}", Poison); } }

        /// <summary>
        /// I say probably because weird things happen in magic
        /// </summary>
        public bool IsProbablyDead { get { return Life < 1 || Poison >= 10 || CommanderDamage.Any(cdi => cdi.Amount >= 21); } }

        private int recastCost;

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

        public ObservableCollection<CommanderDamageItem> CommanderDamage { get; set; }

        public void Refresh()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCommanderGame { get { return CommanderAdditionalCost != 0 || CommanderDamage.Count != 0; } }
    }
}

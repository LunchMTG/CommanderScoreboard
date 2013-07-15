using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;

namespace Commander_Scoreboard
{
    public class Player :INotifyPropertyChanged
    {
        public Player(bool isInCommanderGame = true)
        {
            Life = isInCommanderGame ? 40 : 20;
            IsCommanderGame = isInCommanderGame;
            CommanderDamage = new ObservableCollection<CommanderDamageItem>();
        }

        public string Name { get; set; }
        public int Life { get; set; }
        public string LifeText { get { return string.Format("❤ {0}", Life); } }
        public int Poison { get; set; }
        public string PoisonText { get { return string.Format("☠ {0}", Poison); } }
        public int CommanderAdditionalCost { get; set; }
        public string CommanderAdditionalCostText { get { return string.Format("ⓧ {0}", CommanderAdditionalCost); } }
        public ObservableCollection<CommanderDamageItem> CommanderDamage { get; set; }

        public void Refresh()
        {
            if (PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCommanderGame { get; set; }
        public Visibility ShowCommanderControls { get { return IsCommanderGame ? Visibility.Visible : Visibility.Collapsed; } }
    }
}

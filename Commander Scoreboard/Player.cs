using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Commander_Scoreboard
{
    public class Player :INotifyPropertyChanged
    {
        public Player()
        {
            CommanderDamage = new ObservableCollection<CommanderDamageItem>();
        }

        public string Name { get; set; }
        public int Life { get; set; }
        public int Poison { get; set; }
        public int CommanderAdditionalCost { get; set; }
        public ObservableCollection<CommanderDamageItem> CommanderDamage { get; set; }

        public void Refresh()
        {
            if (PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

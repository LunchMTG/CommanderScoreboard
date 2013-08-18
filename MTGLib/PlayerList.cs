using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MTGLib
{
    public class PlayerList
    {
        public PlayerList(IPlayerNamesStore namesPersister)
        {
            this.namesPersister = namesPersister;
            AvailablePlayers = new ObservableCollection<string>(namesPersister.Load());
            Players = new ObservableCollection<string>();
        }

        public ObservableCollection<string> AvailablePlayers { get; set; }
        
        public ObservableCollection<string> Players { get; set; }

        public bool CanStartGame { get { return Players.Count > 1; } }

        public void Save()
        {
            namesPersister.Save(AvailablePlayers.ToArray());
        }

        IPlayerNamesStore namesPersister { get; set; }

        public bool IsCommanderGame { get; set; }
        public bool ShowPoisonControls { get; set; }
    }
}

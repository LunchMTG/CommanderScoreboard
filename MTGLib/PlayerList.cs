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
            AvailablePlayers = new ObservableCollection<string>(namesPersister.Load());
        }

        public ObservableCollection<string> AvailablePlayers { get; set; }

        public System.Collections.IList Players { get; set; }

        public bool CanStartGame { get { return Players.Count > 1; } }
    }
}

using MTGLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander_Scoreboard
{
    public class PlayerListCache : IPlayerNamesStore
    {
        public PlayerListCache()
        {
            Players = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Players { get; set; }
        public void Save(IEnumerable<string> names)
        {
            if (!Windows.Storage.ApplicationData.Current.RoamingSettings.Containers.ContainsKey("GameSetup"))
                Windows.Storage.ApplicationData.Current.RoamingSettings.CreateContainer("GameSetup", Windows.Storage.ApplicationDataCreateDisposition.Always);
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings.Containers["GameSetup"];

            if (Players == null) return;

            settings.Values["playerlist"] = string.Join(",", names);
        }
        public string[] Load()
        {
            try { return ((string)Windows.Storage.ApplicationData.Current.RoamingSettings.Containers["GameSetup"].Values["playerlist"]).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray(); }
            catch
            {
                Save(new string[] { });
                return new string[] { };
            }
        }
    }
}

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
            //Windows.Storage.ApplicationData.Current.RoamingSettings.CreateContainer("GameSetup", Windows.Storage.ApplicationDataCreateDisposition.Always);
        }

        public ObservableCollection<string> Players { get; set; }
        public void Save(IEnumerable<string> names)
        {
            if (!Windows.Storage.ApplicationData.Current.RoamingSettings.Containers.ContainsKey("GameSetup"))
                Windows.Storage.ApplicationData.Current.RoamingSettings.CreateContainer("GameSetup", Windows.Storage.ApplicationDataCreateDisposition.Always);
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings.Containers["GameSetup"];


            if (Players == null) return;

            settings.Values["playerlist"] = string.Join(",", names);

            //await Windows.Storage.FileIO.WriteTextAsync(createdFile, writeThis);
        }
        public string[] Load()
        {
            try { return ((string)Windows.Storage.ApplicationData.Current.RoamingSettings.Containers["GameSetup"].Values["playerlist"]).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray(); }
            catch { return new string[] { }; }
            finally { Save(new string[] { }); }
        }


    }
}

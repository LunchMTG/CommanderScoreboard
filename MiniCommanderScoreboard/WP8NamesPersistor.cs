using MTGLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniCommanderScoreboard
{
    public class WP8NamesPersistor : IPlayerNamesStore
    {
        string[] IPlayerNamesStore.Load()
        {
            try { return (string[])IsolatedStorageSettings.ApplicationSettings["scores"]; }
            catch { return new string[] { }; }
        }

        void IPlayerNamesStore.Save(IEnumerable<string> names)
        {
            IsolatedStorageSettings.ApplicationSettings["scores"] = names.ToArray();
        }
    }
}

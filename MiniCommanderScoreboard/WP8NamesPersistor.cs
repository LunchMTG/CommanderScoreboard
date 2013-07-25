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
            try
            {
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                using (var Reader = new StreamReader(new IsolatedStorageFileStream("scores", FileMode.Open, fileStorage)))
                {
                    string textFile = Reader.ReadToEnd();
                    return textFile.Split(';');
                }
            }
            catch (Exception)
            {
                return new string[] { };   
            }

        }

        void IPlayerNamesStore.Save(IEnumerable<string> names)
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("scores", FileMode.OpenOrCreate, fileStorage));
            Writer.WriteLine(string.Join(";", names));
            Writer.Close();
        }
    }
}

using MTGLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCommanderScoreboard
{
    public class WP8NamesPersistor : IPlayerNamesStore
    {
        string[] IPlayerNamesStore.Load()
        {
            return new[] { "Owen", "Ross", "Adam", "Scumbag Merek" };
        }

        void IPlayerNamesStore.Save(IEnumerable<string> names)
        {
        }
    }
}

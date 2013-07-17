using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGLib
{
    public interface IPlayerNamesStore
    {
        string[] Load();
        void Save(IEnumerable<string> names);
    }
}

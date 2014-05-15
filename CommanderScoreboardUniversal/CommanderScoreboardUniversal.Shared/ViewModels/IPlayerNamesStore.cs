using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommanderScoreboardUniversal.ViewModels
{
    public interface IPlayerNamesStore
    {
        string[] Load();
        void Save(IEnumerable<string> names);
    }
}

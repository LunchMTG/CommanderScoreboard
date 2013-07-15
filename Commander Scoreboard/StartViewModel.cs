using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander_Scoreboard
{
    public class StartViewModel
    {
        public bool IsCommanderGame { get; set; }
        public IEnumerable<string> Players { get { return PlayerListCache.Players; } }
    }
}

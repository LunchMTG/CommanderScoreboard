using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commander_Scoreboard
{
    public class Player
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public int Poison { get; set; }
        public int CommanderAdditionalCost { get; set; }
        public Dictionary<Player, int> CommanderDamage { get; set; }

    }
}

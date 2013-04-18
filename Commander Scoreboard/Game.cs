using MyToolkit.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Commander_Scoreboard
{
    public class Game
    {
        public Game()
        {
            Players = new List<Player>
            {
                new Player{ Name= "Owen", Life=40, Poison=0, CommanderAdditionalCost=0, CommanderDamage = new Dictionary<Player,int>()},
                new Player{ Name = "Carlos", Life=47, Poison=0, CommanderAdditionalCost=4,CommanderDamage = new Dictionary<Player,int>()},
                new Player{ Name = "Judah", Life=38, Poison=0, CommanderAdditionalCost=2,CommanderDamage = new Dictionary<Player,int>()},
                new Player{ Name = "Ross", Life=20, Poison=0, CommanderAdditionalCost=0,CommanderDamage = new Dictionary<Player,int>()},
                new Player{ Name = "Merek", Life=17, Poison=0, CommanderAdditionalCost=6,CommanderDamage = new Dictionary<Player,int>()},
                new Player{ Name = "Andy", Life=27, Poison=0, CommanderAdditionalCost=2,CommanderDamage = new Dictionary<Player,int>()}
            };

        }

        public IList<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public Player CommanderDamageSource { get; set; }

        public bool IsCommanderGame { get; set; }
        public bool ArePoisonCountersEnabled { get; set; }
        public bool ShowCommands { get { return CurrentPlayer != null; } }

        public RelayCommand Plus1Life { get { return new RelayCommand(() => CurrentPlayer.Life++); } }
    }
}

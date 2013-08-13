using System.Collections.Generic;

namespace MtgLifeCounter.Models
{
    class Player : MVVM.NotifyObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(() => Name, ref _name, value); }
        }

        private int _life;
        public int Life
        {
            get { return _life; }
            set { SetValue(() => Life, ref _life, value); }
        }

        private int _cmdCost;
        public int CmdCost
        {
            get { return _cmdCost; }
            set { SetValue(() => CmdCost, ref _cmdCost, value); }
        }

        private IDictionary<string, int> _cmdDmg;
        public IDictionary<string, int> CmdDmg
        {
            get { return _cmdDmg; }
            set { SetValue(() => CmdDmg, ref _cmdDmg, value); }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetValue(() => IsVisible, ref _isVisible, value); }
        }

        public Player()
        {
            _cmdDmg = new Dictionary<string, int>();
        }
    }
}

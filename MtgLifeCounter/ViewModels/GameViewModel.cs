using DI;
using MtgLifeCounter.Models;
using MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WinRTUtil;

namespace MtgLifeCounter.ViewModels
{
    class GameViewModel : MVVM.NotifyObject
    {
        private static readonly Lazy<GameViewModel> _instance = new Lazy<GameViewModel>(() => new GameViewModel());
        public static GameViewModel Instance { get { return _instance.Value; } }

        private ObservableCollection<Player> _players;
        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set { SetValue(() => Players, ref _players, value); }
        }

        private bool _isCommander;
        public bool IsCommander
        {
            get { return _isCommander; }
            set { SetValue(() => IsCommander, ref _isCommander, value); }
        }

        public ICommand CmdPlay { get { return new RelayCommand(Play, CanPlay); } }
        public ICommand CmdAddPlayer { get { return new RelayCommand(AddPlayer, CanAddPlayer); } }
        public ICommand CmdRemovePlayer { get { return new RelayCommand<Player>(RemovePlayer); } }
        public ICommand CmdNewGame { get { return new RelayCommand(NewGame); } }
        public ICommand CmdHidePlayer { get { return new RelayCommand<Player>(HidePlayer, CanHidePlayer); } }
        public ICommand CmdShowHiddenPlayers { get { return new RelayCommand(ShowHiddenPlayers); } }

        public GameViewModel()
        {
            Players = new ObservableCollection<Player>();
            Players.Add(new Player() { Name = "Defacto" });
            Players.Add(new Player() { Name = "Player2" });
        }

        private void Play()
        {
            NewGame();
            Container.Global.Resolve<INavigationService>().Navigate("GameView", this);
        }

        private bool CanPlay()
        {
            return Players.Count > 1;
        }

        private void AddPlayer()
        {
            Players.Add(new Player() { Name = "Player" + (Players.Count + 1) });
        }

        private bool CanAddPlayer()
        {
            return Players.Count < 9;
        }

        private void RemovePlayer(Player player)
        {
            if (player != null)
                Players.Remove(player);
        }

        private void NewGame()
        {
            int life = IsCommander ? 40 : 20;
            foreach (Player player in Players)
            {
                player.Life = life;
                player.CmdCost = 0;
                player.CmdDmg.Clear();
                player.RaisePropertyChanged("CmdDmg");
                player.IsVisible = true;
            }
        }

        private void ShowHiddenPlayers()
        {
            foreach (Player player in Players)
            {
                player.IsVisible = true;
            }
        }

        private void HidePlayer(Player player)
        {
            player.IsVisible = false;
        }

        private bool CanHidePlayer(Player player)
        {
            return player != null;
        }
    }
}

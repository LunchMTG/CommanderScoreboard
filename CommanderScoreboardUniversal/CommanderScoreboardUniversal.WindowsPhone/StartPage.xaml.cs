using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Windows.UI.Xaml.Controls;
using CommanderScoreboardUniversal.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.System;

namespace CommanderScoreboardUniversal
{
    public partial class StartPage : Page
    {
        private Game _game;
        // Constructor
        public StartPage()
        {
            InitializeComponent();
            DataContext = new PlayerList(new PlayerListCache(), new LicenseInformationWrapper());
        }

        private PlayerList vm { get { return DataContext as PlayerList; } }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as PlayerList).Players = new System.Collections.ObjectModel.ObservableCollection<string>((sender as GridView).SelectedItems.OfType<string>());
        }

        private void AddPlayer(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewPlayerBox.Text))
                vm.AvailablePlayers.Add(NewPlayerBox.Text);
            NewPlayerBox.Text = "";
            vm.Save();
        }

        private void StartGame(bool isCommander)
        {
            var players = vm.Players.Select(name => new Player(isCommander) { Name = name }).ToList();

            for (int i = 0; i < vm.GuestCount; i++)
                players.Add(new Player(isCommander) { Name = "guest " + (i + 1) });

            _game = new Game
             {
                 IsCommanderGame = isCommander,
                 Players = new System.Collections.ObjectModel.ObservableCollection<Player>(players)
             };

            if (_game.Players.Any())
            {
                Frame.Navigate(typeof(MainPage), _game);
            }
        }

        private void StartStandardGame(object sender, EventArgs e)
        {
            StartGame(false);
        }

        private void StartCommanderGame(object sender, EventArgs e)
        {
            StartGame(true);
        }

        private void GotoRateInStore(object sender, EventArgs e)
        {
            //new Microsoft.Phone.Tasks.MarketplaceReviewTask().Show();
        }

        private void SendOwenAnEmail(object sender, EventArgs e)
        {
            //new Microsoft.Phone.Tasks.EmailComposeTask
            //{
            //    To = "owenjohnson@outlook.com",
            //    Subject = "Commander Scoreboard Mini",
            //}.Show();
        }

        private void GotoBuyInStore(object sender, EventArgs e)
        {
            //new Microsoft.Phone.Tasks.MarketplaceDetailTask().Show();
        }



        private void ResumeGame(object sender, EventArgs e)
        {
            if (_game != null)
                Frame.Navigate(typeof(MainPage), _game);
        }

        private void DeletePlayers(object sender, RoutedEventArgs e)
        {
            while (this.PlayerListBox.SelectedItems.Count > 0)
                vm.AvailablePlayers.Remove((string)PlayerListBox.SelectedItems[0]);

            vm.Save();
        }

        private void NewPlayerBox_KeyDown_1(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                AddPlayer(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }
    }
}

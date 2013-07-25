using Microsoft.Phone.Controls;
using MTGLib;
using Ninject;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Phone.Shell;

namespace MiniCommanderScoreboard
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            IKernel kernel = new Ninject.StandardKernel(new MTGLibWp8Module());
            DataContext = kernel.Get<PlayerList>();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private PlayerList vm { get { return DataContext as PlayerList; } }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as PlayerList).Players = new System.Collections.ObjectModel.ObservableCollection<string>((sender as ListBox).SelectedItems.OfType<string>());
        }

        private void AddPlayer(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("AddPlayer.xaml"));
        }

        private void StartCommanderGame(object sender, EventArgs e)
        {
            var players = vm.Players.Select(name => new Player { Name = name }).ToList();

            var game = new Game
            {
                IsCommanderGame = true,
                Players = new System.Collections.ObjectModel.ObservableCollection<Player>(players)
            };
            PhoneApplicationService.Current.State["game"] = game;
            NavigationService.Navigate(new Uri("/Scoreboard.xaml", UriKind.Relative));

        }

        private void StartStandardGame(object sender, EventArgs e)
        {

        }

        private void GotoRateInStore(object sender, EventArgs e)
        {

        }

        private void SendOwenAnEmail(object sender, EventArgs e)
        {

        }

        private void GotoBuyInStore(object sender, EventArgs e)
        {

        }

        private void DeletePlayers(object sender, EventArgs e)
        {
            foreach (string player in PlayerListBox.SelectedItems)
            {
                vm.AvailablePlayers.Remove(player);
                vm.Players.Remove(player);
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
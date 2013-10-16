﻿using Microsoft.Phone.Controls;
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
        }

        private PlayerList vm { get { return DataContext as PlayerList; } }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as PlayerList).Players = new System.Collections.ObjectModel.ObservableCollection<string>((sender as ListBox).SelectedItems.OfType<string>());
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

            var game = new Game
            {
                IsCommanderGame = isCommander,
                Players = new System.Collections.ObjectModel.ObservableCollection<Player>(players)
            };

            if (game.Players.Any())
            {
                PhoneApplicationService.Current.State["game"] = game;
                NavigationService.Navigate(new Uri("/Scoreboard.xaml", UriKind.Relative));
            }
        }

        private void StartStandardGame(object sender, EventArgs e)
        {
            StartGame(false);
        }

        private void GotoRateInStore(object sender, EventArgs e)
        {
            new Microsoft.Phone.Tasks.MarketplaceReviewTask().Show();
        }

        private void SendOwenAnEmail(object sender, EventArgs e)
        {
            new Microsoft.Phone.Tasks.EmailComposeTask
            {
                To = "owenjohnson@outlook.com",
                Subject = "Commander Scoreboard Mini",
            }.Show();
        }

        private void GotoBuyInStore(object sender, EventArgs e)
        {
            new Microsoft.Phone.Tasks.MarketplaceDetailTask().Show();
        }

        private void NewPlayerBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AddPlayer(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void BindableApplicationBarButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            while (PlayerListBox.SelectedItems.Count > 0)
                vm.AvailablePlayers.Remove((string)PlayerListBox.SelectedItems[0]);

            vm.Save();
        }

        private void BindableApplicationBarButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StartGame(true);
        }

        private void BindableApplicationBarButton_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StartGame(false);
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

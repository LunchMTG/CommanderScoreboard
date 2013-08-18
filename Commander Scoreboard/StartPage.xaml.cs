using MTGLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Commander_Scoreboard
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StartPage : Commander_Scoreboard.Common.LayoutAwarePage
    {
        public StartPage()
        {
            this.InitializeComponent();
            DataContext = new PlayerList(new PlayerListCache());

#if DEBUG
            if (CurrentApp.LicenseInformation.IsTrial)
                HideAds();
#else
            if (!CurrentApp.LicenseInformation.IsTrial)
                HideAds();
#endif
        }

        private void HideAds()
        {
            AdControl.Visibility = Visibility.Collapsed;
            AdButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        private PlayerList vm { get { return DataContext as PlayerList; } }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void DeletePlayers(object sender, TappedRoutedEventArgs e)
        {
            var removes = playerPicker.SelectedItems.ToArray();
            foreach (string player in removes)
                vm.AvailablePlayers.Remove(player);

            vm.Save();
        }

        private void StartCommanderGame(object sender, TappedRoutedEventArgs e)
        {


            StartNewGame(true);
        }

        private void StartNewGame(bool isCommander)
        {
            var game = new Game();
            game.IsCommanderGame = isCommander;
            game.ShowPoisonControls = vm.ShowPoisonControls;

            foreach (string player in playerPicker.SelectedItems)
                game.Players.Add(new Player(isCommander) { Name = player });

            if (game.Players.Any())
                Frame.Navigate(typeof(MainPage), game);
        }

        private void StartNotCommanderGame(object sender, TappedRoutedEventArgs e)
        {
            StartNewGame(false);
        }

        private void NewPlayer(object sender, TappedRoutedEventArgs e)
        {
            AddPlayer();
        }

        private void AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(NewPlayerBox.Text))
                vm.AvailablePlayers.Add(NewPlayerBox.Text);
            vm.Save();
        }

        private void NewPlayerBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                AddPlayer();
                NewPlayerBox.Text = "";
                e.Handled = true;
            }
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            HideAds();
        }

        private async void AdButton_Click(object sender, RoutedEventArgs e)
        {
            string r = await CurrentApp.RequestAppPurchaseAsync(true);
            HideAds();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(vm.IsCommanderGame);
        }
    }
}

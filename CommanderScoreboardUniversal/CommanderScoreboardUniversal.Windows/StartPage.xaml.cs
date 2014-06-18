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
using CommanderScoreboardUniversal.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CommanderScoreboardUniversal
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
            DataContext = new PlayerList(new PlayerListCache(),new LicenseInformationWrapper());

//#if DEBUG
//            if (CurrentApp.LicenseInformation.IsTrial)
//                HideAds();
//#else
//            //if (!CurrentApp.LicenseInformation.IsTrial)
//            //    HideAds();
//#endif
        }

        //private void HideAds()
        //{
        //    theAd.Visibility = Visibility.Collapsed;
        //    AdButton.Visibility = Visibility.Collapsed;
        //}

        private PlayerList vm { get { return DataContext as PlayerList; } }

        private void DeletePlayers(object sender, TappedRoutedEventArgs e)
        {
            var removes = playerPicker.SelectedItems.ToArray();
            foreach (string player in removes)
                vm.AvailablePlayers.Remove(player);

            vm.Save();
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

        private void NewPlayerBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                vm.AddPlayer.Execute(null);
                NewPlayerBox.Text = "";
                e.Handled = true;
            }
        }

        //private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        //{
        //    HideAds();
        //}

        private async void AdButton_Click(object sender, RoutedEventArgs e)
        {
            string r = await CurrentApp.RequestAppPurchaseAsync(false);
            //HideAds();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(vm.IsCommanderGame);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            PlayerListCache.Load();
        }

        private StartViewModel vm { get { return DataContext as StartViewModel; } }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            PlayerListCache.Save();
        }

        private void DeletePlayers(object sender, TappedRoutedEventArgs e)
        {
            foreach (string player in playerPicker.SelectedItems)
                vm.Players.Remove(player);
        }

        private void StartCommanderGame(object sender, TappedRoutedEventArgs e)
        {


            StartNewGame(true);
        }

        private void StartNewGame(bool isCommander)
        {
            var game = new Game();
            game.IsCommanderGame = isCommander;

            foreach (string player in playerPicker.SelectedItems)
                game.Players.Add(new Player(isCommander) { Name = player });

            Frame.Navigate(typeof(MainPage), game);
        }

        private void StartNotCommanderGame(object sender, TappedRoutedEventArgs e)
        {
            StartNewGame(false);
        }
    }
}

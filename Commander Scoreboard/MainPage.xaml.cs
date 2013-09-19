using MTGLib;
using System;
using Windows.Foundation;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Commander_Scoreboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // Used to determine the correct height to ensure our custom UI fills the screen.
        private Rect windowBounds;

        public MainPage()
        {
            this.InitializeComponent();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                DataContext = new Game
                {
                    IsCommanderGame = true,
                    Players = new System.Collections.ObjectModel.ObservableCollection<Player>()
                    { 
                        new Player (true){Name = "Owen"},
                        new Player(true){Name= "Adam"},
                        new Player(true){Name= "Judah"},
                        new Player(true) {Name="Ross"}

                    }
                };
            }

            windowBounds = Window.Current.Bounds;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;

            IdlePrevnter = new DisplayRequest();
            IdlePrevnter.RequestActive();
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            IdlePrevnter.RequestRelease();
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void CoinFlip(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("Coin was flipped!", ((int)Math.Round(new Random().NextDouble())) == 0 ? "Tails" : "Heads").ShowAsync();
        }

        public DisplayRequest IdlePrevnter { get; set; }
    }
}

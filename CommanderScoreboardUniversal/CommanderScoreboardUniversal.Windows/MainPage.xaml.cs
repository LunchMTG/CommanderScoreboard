using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommanderScoreboardUniversal.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CommanderScoreboardUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            MakeDesignerVM();
        }

        private void MakeDesignerVM()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled || Debugger.IsAttached)
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
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;


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

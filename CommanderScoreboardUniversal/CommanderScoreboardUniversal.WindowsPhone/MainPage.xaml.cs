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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CommanderScoreboardUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DisplayRequest IdlePrevnter;
        public MainPage()
        {
            this.InitializeComponent();
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
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MTGLib;

namespace MiniCommanderScoreboard
{
    public partial class Scoreboard : PhoneApplicationPage
    {
        public Scoreboard()
        {
            DataContext = PhoneApplicationService.Current.State["game"];
            InitializeComponent();
            FixOrientation(Orientation);

        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            FixOrientation(e.Orientation);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            //DataContext = PhoneApplicationService.Current.State["game"];
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //base.OnNavigatingFrom(e);
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        private void FixOrientation(PageOrientation e)
        {
            switch (e)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.LandscapeRight:
                    Landscape.Storyboard.Begin();
                    ApplicationBar.IsVisible = true;
                    break;
                case PageOrientation.None:
                    break;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitDown:
                case PageOrientation.PortraitUp:
                    Portrait.Storyboard.Begin();
                    ApplicationBar.IsVisible = true;
                    break;
                default:
                    break;
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            var vm = DataContext as Game;
            vm.ShowPoisonControls = !vm.ShowPoisonControls;
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            var vm = DataContext as Game;
            foreach (var player in vm.Players)
            {
                player.Poison = 0;
                player.CommanderAdditionalCost = 0;
                player.CommanderDamage.Clear();
                player.Life = player.IsCommanderGame ? 40 : 20;
                player.Refresh();
            }
        }
    }
}
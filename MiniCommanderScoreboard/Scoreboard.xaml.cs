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

        protected override void OnNavigatedTo(NavigationEventArgs e) { PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled; /*Prevent sleep.*/ }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) { PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled; /*Phones need sleep too.*/        }

        private void FixOrientation(PageOrientation e)
        {
            switch (e)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.LandscapeRight:
                    Landscape.Storyboard.Begin();
                    ApplicationBar.IsVisible = false;
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
    }
}
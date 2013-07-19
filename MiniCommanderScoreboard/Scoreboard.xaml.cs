using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MiniCommanderScoreboard
{
    public partial class Scoreboard : PhoneApplicationPage
    {
        public Scoreboard()
        {
            DataContext = PhoneApplicationService.Current.State["game"];
            InitializeComponent();
        }
    }
}
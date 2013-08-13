using MtgLifeCounter.Models;
using MtgLifeCounter.ViewModels;
using MtgLifeCounter.Views.Behaviors;
using MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
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

namespace MtgLifeCounter.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GameView : MtgLifeCounter.Common.LayoutAwarePage
    {
        public ICommand CmdOnCmderDmgFocused { get { return new RelayCommand<FrameworkElement>(OnCmderDmgFocused); } }
        public ICommand CmdOnOtherFocused { get { return new RelayCommand<FrameworkElement>(OnOtherFocused); } }

        public GameView()
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
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = GameViewModel.Instance;
            base.OnNavigatedTo(e);
        }

        private void WrapGrid_Loaded_1(object sender, RoutedEventArgs e)
        {
            WrapGrid grid = (WrapGrid)sender;
            //grid.SetBinding(WrapGrid.WidthProperty, new Binding() { Path = new PropertyPath("ActualWidth"), Source = this });
            //grid.SetBinding(WrapGrid.HeightProperty, new Binding() { Path = new PropertyPath("ActualHeight"), Source = this });

        }

        private void WrapGrid_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            WrapGrid grid = (WrapGrid)sender;
            grid.Orientation = grid.ActualHeight > grid.ActualWidth ? Orientation.Vertical : Orientation.Horizontal;
        }

        private void btnPlusOne_Click(object sender, RoutedEventArgs e)
        {
            Increment(1);
        }

        private void btnPlusFive_Click(object sender, RoutedEventArgs e)
        {
            Increment(5);
        }

        private void btnMinusOne_Click(object sender, RoutedEventArgs e)
        {
            Increment(-1);
        }

        private void btnMinusFive_Click(object sender, RoutedEventArgs e)
        {
            Increment(-5);
        }

        private void Increment(int i)
        {
            var item = Behaviors.FocusManager.GetFocusedElement(items);
            TextBlock txt = Incremental.FindIncremental(item) as TextBlock;

            if (txt != null)
            {
                if (cmderPlayersPopup.Tag == null)
                {
                    int value = Int32.Parse(txt.Text);
                    txt.Text = (value + i).ToString();
                }
                else
                {
                    Player dealer = (Player)cmderPlayersPopup.Tag;
                    Player receiver = (Player)txt.DataContext;

                    if (receiver.CmdDmg.ContainsKey(dealer.Name))
                    {
                        int newValue = receiver.CmdDmg[dealer.Name] + i;
                        if (newValue == 0)
                            receiver.CmdDmg.Remove(dealer.Name);
                        else
                            receiver.CmdDmg[dealer.Name] = newValue;
                    }
                    else
                        receiver.CmdDmg.Add(dealer.Name, i);
                    receiver.Life -= i;
                    receiver.RaisePropertyChanged("CmdDmg");
                }
            }
        }

        private void btnCmderPlayer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Player player = (Player)btn.DataContext;
            cmderPlayersPopup.IsOpen = false;
            cmderPlayersPopup.Tag = player;
        }

        private void OnCmderDmgFocused(FrameworkElement fe)
        {
            var item = Behaviors.FocusManager.GetFocusedElement(items);
            if (item != null)
            {
                GameViewModel vm = (GameViewModel)DataContext;
                Player currentPlayer = (Player)item.DataContext;

                itemsCmderPlayers.ItemsSource = vm.Players;

                Point incrementsPos = gridIncrementals.TransformToVisual(this).TransformPoint(new Point());
                itemsCmderPlayers.Width = this.ActualWidth;
                cmderPlayersPopup.IsOpen = true;
            }
            else
            {
                cmderPlayersPopup.IsOpen = false;
            }
            btnHidePlayers.CommandParameter = fe.DataContext;
        }

        private void OnOtherFocused(FrameworkElement fe)
        {
            cmderPlayersPopup.IsOpen = false;
            cmderPlayersPopup.Tag = null;
            btnHidePlayers.CommandParameter = fe.DataContext;
        }

        private void btnTopAppBar_Click(object sender, RoutedEventArgs e)
        {   
            topBar.IsOpen = false;
        }
    }
}

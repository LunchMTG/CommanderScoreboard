using MtgLifeCounter.ViewModels;
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

namespace MtgLifeCounter.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GameSetup : MtgLifeCounter.Common.LayoutAwarePage
    {
        public GameSetup()
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

        private void Button_Loaded_1(object sender, RoutedEventArgs e)
        {
            //I wish we had full relative sources :(
            Button btn = (Button)sender;
            btn.SetBinding(Button.CommandProperty, new Binding() { Source = DataContext, Path = new PropertyPath("CmdRemovePlayer") });
            btn.CommandParameter = btn.DataContext;
        }

        private async void btnAddPlayer_Clicked(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    GameViewModel vm = (GameViewModel)DataContext;
                    var container = itemsPlayers.ItemContainerGenerator.ContainerFromIndex(vm.Players.Count - 1);
                    TextBox txtBox = null;
                    DependencyObject dObj = container;
                    while (txtBox == null && dObj != null)
                    {
                        dObj = VisualTreeHelper.GetChild(dObj, 0);
                        txtBox = dObj as TextBox;
                    }

                    if (txtBox != null)
                    {
                        txtBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                        txtBox.SelectAll();
                    }
                });
        }
    }
}

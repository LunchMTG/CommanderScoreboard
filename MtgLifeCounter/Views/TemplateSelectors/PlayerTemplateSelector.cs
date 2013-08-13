using MtgLifeCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MtgLifeCounter.Views.TemplateSelectors
{
    class PlayerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CommanderTemplate { get; set; }
        public DataTemplate SixtyCardTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var parent = VisualTreeHelper.GetParent(container) as FrameworkElement;
            if (parent != null)
            {
                var game = parent.DataContext as GameViewModel;

                return game.IsCommander ? CommanderTemplate : SixtyCardTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}

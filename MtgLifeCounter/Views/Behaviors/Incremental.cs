using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MtgLifeCounter.Views.Behaviors
{
    public static class Incremental
    {
        public static readonly DependencyProperty CanIncrementProperty = DependencyProperty.RegisterAttached("CanIncrement", typeof(bool), typeof(Incremental), new PropertyMetadata(false));

        public static bool GetCanIncrement(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanIncrementProperty);
        }

        public static void SetCanIncrement(DependencyObject obj, bool value)
        {
            obj.SetValue(CanIncrementProperty, value);
        }

        public static FrameworkElement FindIncremental(DependencyObject scope)
        {
            if (scope == null)
                return null;
            if (GetCanIncrement(scope))
                return scope as FrameworkElement;

            int childCount = VisualTreeHelper.GetChildrenCount(scope);
            for (int i = 0; i < childCount; i++)
            {
                var item = FindIncremental(VisualTreeHelper.GetChild(scope, i));
                if (item != null)
                    return item;
            }

            return null;
        }
    }
}

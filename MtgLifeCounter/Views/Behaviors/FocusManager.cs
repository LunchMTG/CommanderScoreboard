using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace MtgLifeCounter.Views.Behaviors
{
    public static class FocusManager
    {
        private static readonly DependencyProperty FocusedElementProperty = DependencyProperty.RegisterAttached("FocusedElement", typeof(FrameworkElement), typeof(FocusManager), new PropertyMetadata(null));

        private static FrameworkElement GetFocusedElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(FocusedElementProperty);
        }

        private static void SetFocusedElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(FocusedElementProperty, value);
        }

        public static FrameworkElement GetFocusedElement(FrameworkElement scope)
        {
            FrameworkElement focused = null;// GetFocusedElement((DependencyObject)scope);
            if (focused == null)
            {
                focused = FindFocusedElement(scope) as FrameworkElement;
                SetFocusedElement(scope, focused);
            }

            return focused;
        }

        private static DependencyObject FindFocusedElement(DependencyObject scope)
        {
            if (scope == null)
                return null;
            if (GetHasFocus(scope))
                return scope;

            int childCount = VisualTreeHelper.GetChildrenCount(scope);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject focused = FindFocusedElement(VisualTreeHelper.GetChild(scope, i));
                if (focused != null)
                    return focused;
            }

            return null;
        }

        public static readonly DependencyProperty HasFocusProperty = DependencyProperty.RegisterAttached("HasFocus", typeof(bool), typeof(FocusManager), new PropertyMetadata(false));

        public static bool GetHasFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasFocusProperty);
        }

        public static void SetHasFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(HasFocusProperty, value);
        }

        public static readonly DependencyProperty CanFocusProperty = DependencyProperty.RegisterAttached("CanFocus", typeof(bool), typeof(FocusManager), new PropertyMetadata(false, CanFocus_PropertyChanged));

        public static bool GetCanFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanFocusProperty);
        }

        public static void SetCanFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(CanFocusProperty, value);
        }

        private static void CanFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe != null)
            {
                bool newValue = (bool)e.NewValue;

                if (newValue)
                {
                    fe.PointerPressed += fe_PointerPressed;
                }
                else
                {
                    fe.PointerPressed -= fe_PointerPressed;
                }
            }
        }

        private static void fe_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            bool hasFocus = !GetHasFocus(fe);
            SetHasFocus(fe, hasFocus);

            if (hasFocus)
                fe.SetValue(Windows.UI.Xaml.Controls.Grid.BackgroundProperty, Application.Current.Resources["ListBoxItemSelectedBackgroundThemeBrush"] as Brush);
            else
                fe.ClearValue(Windows.UI.Xaml.Controls.Grid.BackgroundProperty);

            UnFocusAllButThis(fe);

            ICommand cmd = GetOnFocusedCommand(fe);
            if (cmd != null && cmd.CanExecute(fe))
                cmd.Execute(fe);
        }

        private static void UnFocusAllButThis(DependencyObject fe)
        {
            UnFocusAllButThis(fe, fe);
        }

        private static void UnFocusAllButThis(DependencyObject start, DependencyObject focusedElement)
        {
            if (start == null) return;
            if (GetIsFocusScope(start))
            {
                UnFocusAllButThisWithinThisScope(start, focusedElement);
            }

            UnFocusAllButThis(VisualTreeHelper.GetParent(start), focusedElement);
        }

        private static void UnFocusAllButThisWithinThisScope(DependencyObject start, DependencyObject focusedElement)
        {
            if (start == null)
                return;
            if (GetHasFocus(start) && !Object.Equals(start, focusedElement))
            {
                SetHasFocus(start, false);
                start.ClearValue(Windows.UI.Xaml.Controls.Grid.BackgroundProperty);
                //return;
            }

            int childCount = VisualTreeHelper.GetChildrenCount(start);
            for (int i = 0; i < childCount; i++)
            {
                UnFocusAllButThisWithinThisScope(VisualTreeHelper.GetChild(start, i), focusedElement);
            }
        }

        public static readonly DependencyProperty IsFocusScopeProperty = DependencyProperty.RegisterAttached("IsFocusScope", typeof(bool), typeof(FocusManager), new PropertyMetadata(false));

        public static bool GetIsFocusScope(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusScopeProperty);
        }

        public static void SetIsFocusScope(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusScopeProperty, value);
        }
        
        public static readonly DependencyProperty OnFocusedCommandProperty = DependencyProperty.RegisterAttached("OnFocusedCommand", typeof(ICommand), typeof(FocusManager), new PropertyMetadata(null));

        public static ICommand GetOnFocusedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OnFocusedCommandProperty);
        }

        public static void SetOnFocusedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(OnFocusedCommandProperty, value);
        }
    }
}

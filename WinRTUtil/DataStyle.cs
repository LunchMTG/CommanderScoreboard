using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace WinRTUtil
{
    public class DataStyle : StyleBase
    {
        public static readonly DependencyProperty StyleProperty =
            DependencyProperty.RegisterAttached("Style", typeof(DataStyle), typeof(DataStyle), new PropertyMetadata(null, DataStyle_PropertyChanged));

        public static DataStyle GetStyle(DependencyObject obj)
        {
            return (DataStyle)obj.GetValue(StyleProperty);
        }

        public static void SetStyle(DependencyObject obj, DataStyle value)
        {
            obj.SetValue(StyleProperty, value);
        }

        public ObservableCollection<DataTrigger> Triggers { get; private set; }

        public DataStyle()
        {
            ShouldApplySetters = true;
            Triggers = new ObservableCollection<DataTrigger>();
            Triggers.CollectionChanged += Triggers_CollectionChanged;
        }

        private static void DataStyle_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataStyle oldValue = e.OldValue as DataStyle;
            DataStyle newValue = e.NewValue as DataStyle;

            EventHandler applyStyleHandler = (o, ee) =>
            {
                DataStyle style = (DataStyle)o;
                style.ApplyStyle(d);
            };

            if (oldValue != null)
                oldValue.ShouldApplySettersChanged -= applyStyleHandler;
            if (newValue != null)
            {
                newValue = (DataStyle)newValue.Clone();
                newValue.ShouldApplySettersChanged += applyStyleHandler;
                FrameworkElement fe = d as FrameworkElement;
                if (fe != null)
                {
                    // wait for the data context.
                    fe.Loaded += (a, b) =>
                    {
                        foreach (DataTrigger trigger in newValue.Triggers)
                        {
                            if (trigger.Binding.Source == null)
                            {
                                // set the source to this source. The binding doesn't get this automatically. This also means that we can't reuse our styles, which is why we clone.
                                trigger.Binding.Source = fe.DataContext;
                                trigger.Bind();
                            }
                        }
                    };
                }
            }
        }

        private void Triggers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (DataTrigger trigger in e.OldItems)
                {
                    trigger.ShouldApplySettersChanged -= trigger_ShouldApplySettersChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (DataTrigger trigger in e.NewItems)
                {
                    trigger.ShouldApplySettersChanged += trigger_ShouldApplySettersChanged;
                }
            }
        }

        private void trigger_ShouldApplySettersChanged(object sender, EventArgs e)
        {
            RaiseShouldApplySetterChanged();
        }

        protected internal override void ApplyStyleBase(DependencyObject dObj)
        {
            foreach (DataTrigger trigger in Triggers.Where(t => !t.ShouldApplySetters))
            {
                trigger.ResetStyleBase(dObj);
            }

            base.ApplyStyleBase(dObj);

            foreach (DataTrigger trigger in Triggers.Where(t => t.ShouldApplySetters))
            {
                trigger.ApplyStyleBase(dObj);
            }
        }

        protected internal override void ResetStyleBase(DependencyObject dObj)
        {
            foreach (DataTrigger trigger in Triggers.Where(t => !t.ShouldApplySetters))
            {
                trigger.ResetStyleBase(dObj);
            }

            base.ResetStyleBase(dObj);

            foreach (DataTrigger trigger in Triggers.Where(t => t.ShouldApplySetters))
            {
                trigger.ApplyStyleBase(dObj);
            }
        }

        public override StyleBase Clone()
        {
            DataStyle style = new DataStyle();
            style.Setters.AddRange(Setters);

            foreach (DataTrigger trigger in Triggers)
            {
                style.Triggers.Add((DataTrigger)trigger.Clone());
            }

            return style;
        }
    }

}

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTUtil
{
    public class DataTrigger : StyleBase
    {
        private readonly BindingResolver _resolver;

        public Binding Binding { get; set; }
        public object Value { get; set; }

        public DataTrigger()
        {
            _resolver = new BindingResolver();
            _resolver.EvalChanged += _resolver_EvalChanged;
        }

        public override StyleBase Clone()
        {
            DataTrigger trigger = new DataTrigger();
            trigger.Value = Value;
            trigger.Binding = CloneBinding(Binding);
            trigger.Setters.AddRange(Setters);

            return trigger;
        }

        private static Windows.UI.Xaml.Data.Binding CloneBinding(Binding binding)
        {
            return new Binding()
            {
                Converter = binding.Converter,
                ConverterLanguage = binding.ConverterLanguage,
                ConverterParameter = binding.ConverterParameter,
                ElementName = binding.ElementName,
                Mode = binding.Mode,
                Path = binding.Path,
                RelativeSource = binding.RelativeSource,
                Source = binding.Source
            };
        }

        private void _resolver_EvalChanged(object obj)
        {
            string strValue = Value as string;
            if (strValue != null && obj != null)
                Value = TypeConverter.Convert(strValue, obj.GetType());
            ShouldApplySetters = Object.Equals(obj, Value);
        }

        internal void Bind()
        {
            if (Binding == null)
                _resolver.Eval = null;
            else
            {
                BindingOperations.SetBinding(_resolver, BindingResolver.EvalProperty, Binding);
            }
        }

        class BindingResolver : DependencyObject
        {
            public event Action<object> EvalChanged;

            public static readonly DependencyProperty EvalProperty = DependencyProperty.Register("Eval", typeof(object), typeof(BindingResolver), new PropertyMetadata(null, Eval_PropertyChanged));
            public object Eval
            {
                get { return (object)this.GetValue(EvalProperty); }
                set { this.SetValue(EvalProperty, value); }
            }

            private static void Eval_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                // Typically, I would use DependencyPropertyDescriptor, but that got yanked as well.
                BindingResolver br = (BindingResolver)d;
                br.RaiseEvalChanged(e.NewValue);
            }

            private void RaiseEvalChanged(object newValue)
            {
                var temp = EvalChanged;
                if (temp != null)
                    temp(newValue);
            }
        }
    }

}

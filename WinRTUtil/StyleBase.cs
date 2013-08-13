using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace WinRTUtil
{
    [ContentProperty(Name = "Setters")]
    public abstract class StyleBase : DependencyObject
    {
        private bool _shouldApplySetters;
        public bool ShouldApplySetters
        {
            get { return _shouldApplySetters; }
            set
            {
                if (_shouldApplySetters != value)
                {
                    _shouldApplySetters = value;
                    RaiseShouldApplySetterChanged();
                }
            }
        }
        public List<Setter> Setters { get; private set; }

        public event EventHandler ShouldApplySettersChanged;

        public StyleBase()
        {
            Setters = new List<Setter>();
        }

        public abstract StyleBase Clone();

        public void ApplyStyle(DependencyObject dObj)
        {
            if (ShouldApplySetters)
                ApplyStyleBase(dObj);
            else
                ResetStyleBase(dObj);
        }

        protected internal virtual void ApplyStyleBase(DependencyObject dObj)
        {
            foreach (Setter setter in Setters)
            {
                dObj.SetValue(setter.Property, setter.Value);
            }
        }

        protected internal virtual void ResetStyleBase(DependencyObject dObj)
        {
            foreach (Setter setter in Setters)
            {
                dObj.ClearValue(setter.Property);
            }
        }

        protected void RaiseShouldApplySetterChanged()
        {
            var temp = ShouldApplySettersChanged;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }
    }
}

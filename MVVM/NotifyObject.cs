using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MVVM
{
    public class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(Expression<Func<T>> memberExpression, ref T field, T value, Func<bool> onPropertyChanging = null, Action onPropertyChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)
               || (onPropertyChanging != null
                && !onPropertyChanging()))
                return false;

            field = value;
            RaisePropertyChanged(memberExpression);
         
            if (onPropertyChanged != null)
                onPropertyChanged();

            return true;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            string propertyName = GetPropertyName(expression);
            RaisePropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("memberExpression");
            }
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Lambda must return a property.");
            }

            return body.Member.Name;
        }
    }
}

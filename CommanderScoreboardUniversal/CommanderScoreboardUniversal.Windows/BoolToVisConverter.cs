using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CommanderScoreboardUniversal
{
    public class BoolToVisConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

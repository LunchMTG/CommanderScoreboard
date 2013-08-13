using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MtgLifeCounter.Views.Converters
{
    class CommanderDamageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IDictionary<string, int> values = (IDictionary<string, int>)value;
            StringBuilder sb = new StringBuilder();

            foreach (var kv in values)
            {
                sb.AppendFormat("{0}: {1}{2}", kv.Key, kv.Value, Environment.NewLine);
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

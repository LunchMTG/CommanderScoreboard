using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTUtil
{
    /// <summary>
    /// Simple type converter to parse string values.
    /// </summary>
    static class TypeConverter
    {
        public static object Convert(string text, Type type)
        {
            if (type == typeof(string))
                return text;
            if (type == typeof(bool))
                return Boolean.Parse(text);
            if (type == typeof(int))
                return Int32.Parse(text);
            throw new NotSupportedException(String.Format("Type {0} is not supported", type.Name));
        }
    }
}

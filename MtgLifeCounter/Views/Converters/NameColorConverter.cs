using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MtgLifeCounter.Views.Converters
{
    class NameColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string name = (string)value ?? String.Empty;

            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash cHash = provider.CreateHash();
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(name, BinaryStringEncoding.Utf16BE);
            cHash.Append(buffer);
            IBuffer hashedBuffer = cHash.GetValueAndReset();
            byte[] hashBytes = null;
            CryptographicBuffer.CopyToByteArray(hashedBuffer, out hashBytes);
            //int hash = name.GetHashCode();
            //byte[] hashBytes = BitConverter.GetBytes(hash);
            return new SolidColorBrush(Color.FromArgb(0xCC, hashBytes[0], hashBytes[1], hashBytes[2]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

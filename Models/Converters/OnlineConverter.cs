using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VKDesktop.Models.Converters
{
    public class OnlineConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var isOnline = (bool)value;
            return (isOnline ? "Онлайн" : "");
            // Do the conversion from bool to visibility
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
            // Do the conversion from visibility to bool
        }

    }

}

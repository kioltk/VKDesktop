using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VKDesktop.Models.Converters
{
    public class UnreadMessageColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string ccode;
            Color clr;
            SolidColorBrush scb;
            var isUnread = (bool)value;
            if (isUnread)
            {
                ccode = "#DAE1E8";
                clr = (Color)ColorConverter.ConvertFromString(ccode);
                scb = new SolidColorBrush(clr);
                return scb;
            }

            ccode = "#00FFFF00";
            clr = (Color)ColorConverter.ConvertFromString(ccode);
            scb = new SolidColorBrush(clr);
            return scb;
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


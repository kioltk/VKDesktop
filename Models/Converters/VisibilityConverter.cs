using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
namespace VKDesktop.Models.Converters
{
    public class VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;
            return (isVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
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

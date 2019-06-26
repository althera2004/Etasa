using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EtasaDesktop.Resources.Converters
{
    public class HexColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Transparent);
            else {
                try
                {
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                }
                catch (Exception)
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            return brush.Color.ToString();
        }
    }
}

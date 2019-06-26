using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EtasaDesktop.Resources.Converters
{
    public class VisibilityEmptyConverter : IValueConverter
    {
        public bool Collapse { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                var text = value as string;
                if (!String.IsNullOrWhiteSpace(text))
                    return Visibility.Visible;
            }
            else if(value is IEnumerable<object>)
            {
                var enumerable = value as IEnumerable<object>;
                if (value != null && enumerable.Count() > 0)
                    return Visibility.Visible;
            }
            else
            {
                if(value != null)
                    return Visibility.Visible;
            }


            if (Collapse)
                return Visibility.Collapsed;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

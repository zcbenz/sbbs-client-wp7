using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace sbbs_client_wp7
{
    public class StampDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds((int)value).ToLocalTime();
            DateTime now = DateTime.Now;

            if (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day)
                return date.ToString("HH:mm", culture);
            else if (date.Year == now.Year)
                return date.ToString("M月d日 HH:mm", culture);
            else
                return date.ToString("yyyy年M月d日", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

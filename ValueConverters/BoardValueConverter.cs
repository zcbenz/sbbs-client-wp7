using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;

namespace sbbs_client_wp7
{
    public class LeafBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool leaf = (bool)value;

            return leaf ? Application.Current.Resources["PhoneAccentBrush"] : Application.Current.Resources["PhoneSubtleBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

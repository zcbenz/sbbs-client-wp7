using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;

namespace sbbs_client_wp7
{
    public class BoolAccentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reverse = parameter != null;
            return ((bool)value ^ reverse) ? Application.Current.Resources["PhoneAccentBrush"] : Application.Current.Resources["PhoneForegroundBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class BoolVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reverse = parameter != null;
            return ((bool)value ^ reverse) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class BoolReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}

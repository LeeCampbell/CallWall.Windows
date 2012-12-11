using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CallWall
{
    public sealed class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool)
                flag = (bool)value;
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue && nullable.Value;
            }
            return flag ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value != Visibility.Visible;
            return false;
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace CallWall.Windows.Shell.ProfileDashboard
{
    public sealed class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (DateTimeOffset)value;
            var diff = DateTimeOffset.Now - input;

            if (diff.Days > 28)
                return input.ToString("MMM-yy", culture);
            if (diff.Days > 6)
            {
                var weeks = diff.Days / 7;
                return string.Format(culture, "{0}w", weeks);
            }
            if (diff.Days > 0)
                return string.Format(culture, "{0}d", diff.Days);

            return input.ToString("hh:mm", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace CallWall.ProfileDashboard.Communication
{
    public sealed class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (DateTimeOffset)value;
            
            if (input.Date == DateTimeOffset.Now.Date)
            {
                return input.ToString("hh:mm");
            }
            var diff = DateTimeOffset.Now - input;
            if(diff.Days==1)
            {
                return "1d";
            }
            return string.Format("{0}d", diff.Days);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
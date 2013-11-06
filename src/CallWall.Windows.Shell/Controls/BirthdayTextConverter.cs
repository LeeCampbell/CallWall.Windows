using System;
using System.Globalization;
using System.Windows.Data;

namespace CallWall.Windows.Shell.Controls
{
    //  i i i                             i i i       
    // -------      28-Jan-1978          -------      28-Jan-1978
    //|       |     40th was Tuesday    |       |     (40)
    //---------                         ---------     
    //TODO: I want to be able to say
    // 40th was last [weekday]
    // 40th was [week]
    // 40th was yesterday
    // 40th today
    // 40th tomorrow
    // 40th this [weekday]
    // 40th next [weekday]
    // 40th in 10 days
    // 40th next month
    // (40)

    public sealed class BirthdayTextConverter : IValueConverter
    {
        private readonly Func<DateTime> _getToday;

        public BirthdayTextConverter()
            : this(() => DateTime.Today)
        {
        }

        //Used only for testing.
        public BirthdayTextConverter(Func<DateTime> getToday)
        {
            _getToday = getToday;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dob = (DateTime?)value;
            return dob.HasValue ? dob.Value.ToLongDateString() : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

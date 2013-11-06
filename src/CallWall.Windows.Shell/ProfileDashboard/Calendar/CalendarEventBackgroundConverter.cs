using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using CallWall.Windows.Contract.Calendar;

namespace CallWall.Windows.Shell.ProfileDashboard.Calendar
{
    public sealed class CalendarEventBackgroundConverter : IValueConverter
    {
        public Brush FutureBrush { get; set; }
        public Brush CurrentBrush { get; set; }
        public Brush PastBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var calendarEvent = (ICalendarEvent)value;
            if (calendarEvent.End < DateTimeOffset.Now)
            {
                return PastBrush;
            }
            if (calendarEvent.Start > DateTimeOffset.Now.Date.AddDays(1))
            {
                return FutureBrush;
            }

            return CurrentBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
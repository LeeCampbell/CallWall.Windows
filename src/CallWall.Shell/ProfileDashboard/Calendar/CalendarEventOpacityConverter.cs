using System;
using System.Globalization;
using System.Windows.Data;
using CallWall.Contract.Calendar;

namespace CallWall.ProfileDashboard.Calendar
{
    public sealed class CalendarEventOpacityConverter : IValueConverter
    {
        public double FutureOpacity { get; set; }
        public double CurrentOpacity { get; set; }
        public double PastOpacity { get; set; }

        public CalendarEventOpacityConverter()
        {
            FutureOpacity = 1.0;
            CurrentOpacity = 1.0;
            PastOpacity = 1.0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var calendarEvent = (ICalendarEvent)value;
            if (calendarEvent.End < DateTimeOffset.Now)
            {
                return PastOpacity;
            }
            if (calendarEvent.Start > DateTimeOffset.Now)
            {
                return FutureOpacity;
            }

            return CurrentOpacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Calendar;

namespace CallWall.Windows.Shell.ProfileDashboard.Calendar
{
    public sealed class CalendarQueryAggregator : ICalendarQueryAggregator
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ICalendarQueryProvider> _calendarQueryProviders;

        public CalendarQueryAggregator(ILoggerFactory loggerFactory, IEnumerable<ICalendarQueryProvider> calendarQueryProviders)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _calendarQueryProviders = calendarQueryProviders;
        }

        public IObservable<ICalendarEvent> Search(IProfile activeProfile)
        {
            return from provider in _calendarQueryProviders.ToObservable()
                   from calendarEvent in provider.LoadCalendar(activeProfile)
                                           .Log(_logger, "provider.LoadCalendar(activeProfile)")
                                           .Catch<ICalendarEvent, Exception>(ex =>
                                            {
                                                _logger.Error(ex, "{0} failed loading calendar", provider.GetType().Name);
                                                return Observable.Empty<ICalendarEvent>();
                                            })
                   select calendarEvent;
        }
    }
}

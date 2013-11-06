using System;

namespace CallWall.Windows.Contract.Calendar
{
    public interface ICalendarQueryProvider
    {
        IObservable<ICalendarEvent> LoadCalendar(IProfile activeProfile);
    }
}

using System;

namespace CallWall.Contract.Calendar
{
    public interface ICalendarQueryProvider
    {
        IObservable<ICalendarEvent> LoadCalendar(IProfile activeProfile);
    }
}

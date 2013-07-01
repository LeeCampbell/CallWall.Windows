using System;
using CallWall.Contract;
using CallWall.Contract.Calendar;

namespace CallWall.ProfileDashboard.Calendar
{
    public interface ICalendarQueryAggregator
    {
        IObservable<ICalendarEvent> Search(IProfile profile);
    }
}
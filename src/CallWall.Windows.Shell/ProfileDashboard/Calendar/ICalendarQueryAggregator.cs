using System;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Calendar;

namespace CallWall.Windows.Shell.ProfileDashboard.Calendar
{
    public interface ICalendarQueryAggregator
    {
        IObservable<ICalendarEvent> Search(IProfile profile);
    }
}
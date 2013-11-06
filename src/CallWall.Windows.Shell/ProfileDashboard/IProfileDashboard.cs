using System;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Calendar;
using CallWall.Windows.Contract.Contact;
using CallWall.Windows.Shell.ProfileDashboard.Communication;
using CallWall.Windows.Shell.ProfileDashboard.Pictures;

namespace CallWall.Windows.Shell.ProfileDashboard
{
    public interface IProfileDashboard : IDisposable
    {
        void Load(IProfile profile);

        string ActivatedIdentity { get; }

        IObservable<IContactProfile> Contact { get; }

        IObservable<Message> Messages { get; }

        IObservable<Album> PictureAlbums { get; }

        IObservable<ICalendarEvent> CalendarEvents { get; }
    }
}
using System;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Contact;

namespace CallWall.Windows.Shell.ProfileDashboard.Contact
{
    public interface IContactQueryAggregator
    {
        IObservable<IContactProfile> Search(IProfile activeProfile);
    }
}
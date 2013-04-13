using System;
using CallWall.Contract;
using CallWall.Contract.Contact;

namespace CallWall.ProfileDashboard.Contact
{
    public interface IContactQueryAggregator
    {
        IObservable<IContactProfile> Search(IProfile activeProfile);
    }
}
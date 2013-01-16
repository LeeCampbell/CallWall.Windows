using System;
using CallWall.Contract;

namespace CallWall.ProfileDashboard.Contact
{
    public interface IContactQueryAggregator
    {
        IObservable<IContactProfile> Search(IProfile activeProfile);
    }
}
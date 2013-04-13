using System;
using CallWall.Contract;

namespace CallWall.ProfileDashboard.Pictures
{
    public interface IPictureQueryAggregator
    {
        IObservable<Album> Search(IProfile activeProfile);
    }
}
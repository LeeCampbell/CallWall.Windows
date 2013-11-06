using System;
using CallWall.Windows.Contract;

namespace CallWall.Windows.Shell.ProfileDashboard.Pictures
{
    public interface IPictureQueryAggregator
    {
        IObservable<Album> Search(IProfile activeProfile);
    }
}
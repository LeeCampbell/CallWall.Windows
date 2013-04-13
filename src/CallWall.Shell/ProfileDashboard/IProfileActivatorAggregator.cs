using System;

namespace CallWall.ProfileDashboard
{
    public interface IProfileActivatorAggregator
    {
        IObservable<Contract.IProfile> ProfileActivated();
    }
}
using System;

namespace CallWall.Windows.Shell.ProfileDashboard
{
    public interface IProfileActivatorAggregator
    {
        IObservable<Contract.IProfile> ProfileActivated();
    }
}
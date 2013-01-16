using System;

namespace CallWall.ProfileDashboard
{
    public interface IProfileActivator
    {
        IObservable<Contract.IProfile> ProfileActivated();
    }
}
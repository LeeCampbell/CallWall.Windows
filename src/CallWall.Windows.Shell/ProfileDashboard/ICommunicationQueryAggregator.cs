using System;
using CallWall.Windows.Contract;
using CallWall.Windows.Shell.ProfileDashboard.Communication;

namespace CallWall.Windows.Shell.ProfileDashboard
{
    public interface ICommunicationQueryAggregator
    {
        IObservable<Message> Search(IProfile activeProfile);
    }
}
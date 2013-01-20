using System;
using CallWall.Contract;

namespace CallWall.ProfileDashboard.Communication
{
    public interface ICommunicationQueryAggregator
    {
        IObservable<MessageViewModel> Search(IProfile activeProfile);
    }
}
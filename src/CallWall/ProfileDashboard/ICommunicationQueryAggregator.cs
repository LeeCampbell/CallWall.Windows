using System;
using CallWall.Contract;

namespace CallWall.ProfileDashboard.Communication
{
    public interface ICommunicationQueryAggregator
    {
        IObservable<Message> Search(IProfile activeProfile);
    }
}
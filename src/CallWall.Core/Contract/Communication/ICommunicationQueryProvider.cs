using System;

namespace CallWall.Contract.Communication
{
    public interface ICommunicationQueryProvider
    {
        IObservable<IMessage> Messages(IProfile activeProfile);
    }
}

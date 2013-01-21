using System;

namespace CallWall.Contract.Communication
{
    public interface ICommunicationQueryProvider
    {
        IObservable<IMessage> LoadMessages(IProfile activeProfile);
    }
}

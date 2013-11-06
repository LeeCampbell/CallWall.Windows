using System;

namespace CallWall.Windows.Contract.Communication
{
    public interface ICommunicationQueryProvider
    {
        IObservable<IMessage> LoadMessages(IProfile activeProfile);
    }
}

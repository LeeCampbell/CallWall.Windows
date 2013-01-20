using System.Collections.Generic;

namespace CallWall.Contract.Communication
{
    public interface ICommunicationProfile
    {
        IEnumerable<IMessage> Message { get; }
    }
}

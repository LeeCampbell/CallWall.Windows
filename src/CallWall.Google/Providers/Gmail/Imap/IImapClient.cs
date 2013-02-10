using System;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    public interface IImapClient : IDisposable
    {
        bool Connect(string sHost, int nPort);
        bool Authenticate(string user, string accessToken);
        IObservable<IMessage> FindEmailsFromOrTo(string emailAddress);
        bool SelectFolder(string folder);
    }
}
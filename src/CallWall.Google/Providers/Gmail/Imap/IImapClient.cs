using System;
using System.Collections.Generic;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    public interface IImapClient : IDisposable
    {
        bool Connect(string sHost, int nPort);
        bool Authenticate(string user, string accessToken);
        bool SelectFolder(string folder);
        IObservable<IList<ulong>> FindEmailIds(string query);
        IObservable<IMessage> FetchEmailSummaries(IEnumerable<ulong> messageIds);
    }
}
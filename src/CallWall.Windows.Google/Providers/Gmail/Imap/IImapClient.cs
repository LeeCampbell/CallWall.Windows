using System;
using System.Collections.Generic;
using CallWall.Windows.Contract.Communication;

namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    public interface IImapClient : IDisposable
    {
        IObservable<bool> Connect(string sHost, int nPort);
        IObservable<bool> Authenticate(string user, string accessToken);
        IObservable<bool> SelectFolder(string folder);
        IObservable<IList<ulong>> FindEmailIds(string query);
        IObservable<IMessage> FetchEmailSummaries(IEnumerable<ulong> messageIds, IEnumerable<string> fromAddresses);
    }
}
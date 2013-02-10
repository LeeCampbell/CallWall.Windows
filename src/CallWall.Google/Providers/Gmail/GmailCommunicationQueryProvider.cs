using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Authentication;
using CallWall.Contract;
using CallWall.Contract.Communication;
using CallWall.Google.Authorization;
using CallWall.Google.Providers.Gmail.Imap;

namespace CallWall.Google.Providers.Gmail
{
    public sealed class GmailCommunicationQueryProvider : ICommunicationQueryProvider
    {
        private readonly IImapClient _imapClient;
        private readonly IGoogleAuthorization _authorization;

        //TODO: Inject an ImapClientFactory
        public GmailCommunicationQueryProvider(IImapClient imapClient, IGoogleAuthorization authorization)
        {
            _imapClient = imapClient;
            _authorization = authorization;
        }

        //TODO: Only run if the email scope is Authorized
        public IObservable<IMessage> LoadMessages(IProfile activeProfile)
        {
            return from token in _authorization.RequestAccessToken()
                   from message in SearchImap(activeProfile, token)
                   select message;
        }

        private IObservable<IMessage> SearchImap(IProfile activeProfile, string accessToken)
        {
            return Observable.Create<IMessage>(
                o =>
                {
                    if (_imapClient.Connect("imap.gmail.com", 993))
                    {
                        if (_imapClient.Authenticate(@"lee.ryan.campbell@gmail.com", accessToken))
                        {
                            if (_imapClient.SelectFolder("[Gmail]/All Mail"))
                            {
                                return activeProfile.Identifiers
                                    .Select(id => _imapClient.FindEmailsFromOrTo(id.Value))
                                    .Concat()
                                    .Subscribe(o);
                            }
                        }
                        o.OnError(new AuthenticationException("Failed to authenticate for Gmail search."));
                        return Disposable.Empty;
                    }
                    o.OnError(new IOException("Failed to connect to Gmail IMAP server."));
                    return Disposable.Empty;
                });
        }
    }
}

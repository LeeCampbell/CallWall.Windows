using System;
using System.Collections.Generic;
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
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;

        //TODO: Inject an ImapClientFactory
        public GmailCommunicationQueryProvider(IImapClient imapClient, IGoogleAuthorization authorization, ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _imapClient = imapClient;
            _authorization = authorization;
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger();
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
                        //HACK:This user account needs to be sourced from the Google Contacts end point https://www.google.com/m8/feeds/contacts/default/full
                        if (_imapClient.Authenticate(@"lee.ryan.campbell@gmail.com", accessToken))
                        {
                            if (_imapClient.SelectFolder("[Gmail]/All Mail"))
                            {
                                var distinctOrderedIds = activeProfile.Identifiers
                                    .Select(id => _imapClient.FindEmailIds(id.Value))
                                    .Merge(_schedulerProvider.Concurrent)
                                    .Aggregate(
                                        new SortedSet<ulong>(),
                                        (set, newValues) =>
                                        {
                                            set.UnionWith(newValues);
                                            return set;
                                        })
                                        .Log(_logger, "distinctOrderedIds");

                                var q = from allIds in distinctOrderedIds
                                        from email in _imapClient.FetchEmailSummaries(allIds.Reverse().Take(15))
                                        select email;
                                return q.Subscribe(o);

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

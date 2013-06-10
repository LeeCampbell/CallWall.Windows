using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Authentication;
using CallWall.Contract;
using CallWall.Contract.Communication;
using CallWall.Google.AccountConfiguration;
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

        public IObservable<IMessage> LoadMessages(IProfile activeProfile)
        {
            return from hasAccess in Observable.Return(_authorization.Status.IsAuthorized).Where(isAuth => isAuth)
                   from token in _authorization.RequestAccessToken(GoogleResource.Gmail)
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
                                //TODO: Potentially this could be a single serach instead of sending n requests.
                                //var distinctOrderedIds = activeProfile.Identifiers
                                //    .Select(id => _imapClient.FindEmailIds(id.Value))
                                //    .Merge(_schedulerProvider.Concurrent)
                                //    .Aggregate(
                                //        new SortedSet<ulong>(),
                                //        (set, newValues) =>
                                //        {
                                //            set.UnionWith(newValues);
                                //            return set;
                                //        })
                                //        .Log(_logger, "distinctOrderedIds");


                                var searchQuery = string.Join(" OR ",
                                                              activeProfile.Identifiers.Select(
                                                                  id => string.Format("\"{0}\"", id.Value)));

                                var distinctOrderedIds = _imapClient.FindEmailIds(searchQuery)
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

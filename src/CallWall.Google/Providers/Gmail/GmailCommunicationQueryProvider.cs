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
        private readonly Func<IImapClient> _imapClientFactory;
        private readonly IGoogleAuthorization _authorization;
        private readonly ILogger _logger;

        public GmailCommunicationQueryProvider(Func<IImapClient> imapClientFactory, IGoogleAuthorization authorization, ILoggerFactory loggerFactory)
        {
            _imapClientFactory = imapClientFactory;
            _authorization = authorization;
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
            return Observable.Using(_imapClientFactory, imapClient => SearchImap(imapClient, activeProfile, accessToken));
        }
        //private IObservable<IMessage> SearchImap(IImapClient imapClient, IProfile activeProfile, string accessToken)
        //{
        //        return Observable.Create<IMessage>(
        //        o =>
        //        {
        //            if (imapClient.Connect("imap.gmail.com", 993))
        //            {
        //                //HACK:This user account needs to be sourced from the Google Contacts end point https://www.google.com/m8/feeds/contacts/default/full
        //                if (imapClient.Authenticate(@"lee.ryan.campbell@gmail.com", accessToken))
        //                {
        //                    if (imapClient.SelectFolder("[Gmail]/All Mail"))
        //                    {
        //                        var searchQuery = string.Join(" OR ",
        //                                                      activeProfile.Identifiers.Select(
        //                                                          id => string.Format("\"{0}\"", id.Value)));

        //                        var distinctOrderedIds = imapClient.FindEmailIds(searchQuery)
        //                                .Log(_logger, "distinctOrderedIds");

        //                        var q = from allIds in distinctOrderedIds
        //                                from email in imapClient.FetchEmailSummaries(allIds.Reverse().Take(15))
        //                                select email;
        //                        return q.Subscribe(o);

        //                    }
        //                }
        //                o.OnError(new AuthenticationException("Failed to authenticate for Gmail search."));
        //                return Disposable.Empty;
        //            }
        //            o.OnError(new IOException("Failed to connect to Gmail IMAP server."));
        //            return Disposable.Empty;
        //        });
        //}

        private string ToSearchQuery(IProfile activeProfile)
        {
            return string.Join(" OR ",activeProfile.Identifiers.Select(id => string.Format("\"{0}\"", id.Value)));
        }
        private IObservable<IMessage> SearchImap(IImapClient imapClient, IProfile activeProfile, string accessToken)
        {
            return from isConnected in imapClient.Connect("imap.gmail.com", 993)
                                                 .Select(isConnectedd =>
                                                     {
                                                         if (isConnectedd) return true;
                                                         throw new IOException("Failed to connect to Gmail IMAP server.");
                                                     })
                   from isAuthenticated in imapClient.Authenticate(@"lee.ryan.campbell@gmail.com", accessToken)
                                                     .Select(isAuthenticatedd =>
                                                     {
                                                         if (isAuthenticatedd) return true;
                                                         throw new AuthenticationException("Failed to authenticate for Gmail search.");
                                                     })       
                   from isSelected in imapClient.SelectFolder("[Gmail]/All Mail")
                   let queryText = ToSearchQuery(activeProfile)
                   from emailIds in imapClient.FindEmailIds(queryText)
                   from email in imapClient.FetchEmailSummaries(emailIds.Reverse().Take(15))
                   select email;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Contact;
using CallWall.Windows.Google.AccountConfiguration;
using CallWall.Windows.Google.Authorization;
using CallWall.Windows.Web;

namespace CallWall.Windows.Google.Providers.Contacts
{
    public sealed class GoogleContactQueryProvider : IContactQueryProvider, IGoogleContactQueryProvider, IDisposable
    {
        private readonly IDisposable _currentUserSubscription;
        private readonly IGoogleAuthorization _authorization;
        private readonly IHttpClient _httpClient;
        private readonly IGoogleContactProfileTranslator _translator;
        private readonly ILogger _logger;
        private readonly BehaviorSubject<GoogleUser> _currentUser = new BehaviorSubject<GoogleUser>(null);

        public GoogleContactQueryProvider(IGoogleAuthorization authorization,
                                          IHttpClient httpClient,
                                          IGoogleContactProfileTranslator translator,
                                          ILoggerFactory loggerFactory)
        {
            _authorization = authorization;
            _httpClient = httpClient;
            _translator = translator;
            _logger = loggerFactory.CreateLogger(GetType());

            //HACK: Don't do work in ctors
            _currentUserSubscription = _authorization.PropertyChanges(a => a.Status)
                          .StartWith(_authorization.Status)
                          .Select(status => status.IsAuthorized ? GetCurrentUser() : Observable.Return<GoogleUser>(null))
                          .Switch()
                          .Subscribe(_currentUser);
        }

        public IObservable<GoogleUser> CurrentUser { get { return _currentUser.AsObservable(); } }

        public IObservable<IContactProfile> LoadContact(IProfile activeProfile)
        {
            return from contact in activeProfile.Identifiers.Select(LoadContact).Merge() //Fires many requests off to hopefully get a matching contact
                   from taggedContact in EnrichTags(contact)
                   select taggedContact;
        }

        private IObservable<GoogleUser> GetCurrentUser()
        {
            var query = from accessToken in _authorization.RequestAccessToken(GoogleResource.Contacts)
                        from request in Observable.Return(CreateRequestParams(accessToken))
                        from response in _httpClient.GetResponse(request)
                        select _translator.GetUser(response);
            return query;
        }

        

        private IObservable<IGoogleContactProfile> LoadContact(IPersonalIdentifier personalIdentifier)
        {
            return (
                       from hasAccess in Observable.Return(_authorization.Status.IsAuthorized).Where(isAuth => isAuth).Log(_logger, "1")
                       from accessToken in _authorization.RequestAccessToken(GoogleResource.Contacts).Log(_logger, "2")
                       from request in Observable.Return(CreateRequestParams(personalIdentifier, accessToken)).Log(_logger, "3")
                       from response in _httpClient.GetResponse(request)
                       select _translator.Translate(response, accessToken)
                   ).Where(profile => profile != null)
                .Log(_logger, string.Format("LoadContact({0})", personalIdentifier.Value))
                .Take(1);
        }

        public IObservable<IGoogleContactProfile> EnrichTags(IGoogleContactProfile contactProfile)
        {
            //TODO: This should fetch any extra pages of groups//TODO: The groups can be cached as they are related to the logged in user. I would imagine that we can safely cache for 1minute.
            return (
                       from hasAccess in Observable.Return(_authorization.Status.IsAuthorized).Where(isAuth => isAuth)
                       from accessToken in _authorization.RequestAccessToken(GoogleResource.Contacts)
                       from request in Observable.Return(CreateConactGroupRequestParams(accessToken))
                       from response in _httpClient.GetResponse(request)
                       select _translator.AddTags(contactProfile, response)
                   )
                .Log(_logger, string.Format("EnrichTags({0})", contactProfile.FullName))
                .Take(1);
        }

        private static HttpRequestParameters CreateRequestParams(IPersonalIdentifier personalIdentifier, string accessToken)
        {
            var param = CreateRequestParams(accessToken);

            var query = personalIdentifier.Value ?? string.Empty;
            param.QueryStringParameters.Add("q", query);

            return param;
        }

        private static HttpRequestParameters CreateRequestParams(string accessToken)
        {
            var param = new HttpRequestParameters(@"https://www.google.com/m8/feeds/contacts/default/full");
            //TODO:Validate that I should be passing this in the query string. Surly I want this encoded in the POST stream -LC
            param.QueryStringParameters.Add("access_token", accessToken);
            param.Headers.Add("GData-Version", "3.0");
            return param;
        }

        private static HttpRequestParameters CreateConactGroupRequestParams(string accessToken)
        {
            var param = new HttpRequestParameters(@"https://www.google.com/m8/feeds/groups/default/full");
            //TODO:Validate that I should be passing this in the query string. Surly I want this encoded in the POST stream -LC
            param.QueryStringParameters.Add("access_token", accessToken);
            param.Headers.Add("GData-Version", "3.0");

            return param;
        }

        public void Dispose()
        {
            _currentUserSubscription.Dispose();
        }
    }

    public interface IGoogleContactQueryProvider : IContactQueryProvider
    {
        IObservable<GoogleUser> CurrentUser { get; }
    }
    public sealed class GoogleUser
    {
        public GoogleUser(string currentUser, IEnumerable<string> emailAddresses)
        {
            Id = currentUser;
            EmailAddresses = emailAddresses.ToArray();
        }
        public string Id { get; private set; }
        public string[] EmailAddresses { get; private set; }
    }
}

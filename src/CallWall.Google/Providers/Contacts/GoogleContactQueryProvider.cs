using System;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.Google.Authorization;
using CallWall.Web;

namespace CallWall.Google.Providers.Contacts
{
    public sealed class GoogleContactQueryProvider : IContactQueryProvider
    {
        private readonly IGoogleAuthorization _authorization;
        private readonly IHttpClient _httpClient;
        private readonly IGoogleContactProfileTranslator _translator;
        private readonly ILogger _logger;

        public GoogleContactQueryProvider(IGoogleAuthorization authorization, IHttpClient httpClient, IGoogleContactProfileTranslator translator, ILoggerFactory loggerFactory)
        {
            _authorization = authorization;
            _httpClient = httpClient;
            _translator = translator;
            _logger = loggerFactory.CreateLogger();
        }

        public IObservable<IContactProfile> LoadContact(IProfile activeProfile)
        {
            return from contact in activeProfile.Identifiers.Select(LoadContact).Merge() //Fires many requests off to hopefully get a matching contact
                   from taggedContact in EnrichTags(contact)
                   select taggedContact;
        }

        private IObservable<IGoogleContactProfile> LoadContact(IPersonalIdentifier personalIdentifier)
        {
            return (
                       from accessToken in _authorization.RequestAccessToken()
                       from request in Observable.Return(CreateRequestParams(personalIdentifier, accessToken))
                       from response in _httpClient.GetResponse(request)
                       select _translator.Translate(response, accessToken)
                   ).Where(profile => profile != null)
                .Log(_logger, string.Format("LoadContact({0})", personalIdentifier.Value))
                .Take(1);
        }

        public IObservable<IGoogleContactProfile> EnrichTags(IGoogleContactProfile contactProfile)
        {
            //TODO: This should fetch any extra pages of groups
            //TODO: The groups can be cached as they are related to the logged in user. I would imagine that we can safely cache for 1minute.
            return (
                       from accessToken in _authorization.RequestAccessToken()
                       from request in Observable.Return(CreateConactGroupRequestParams(accessToken))
                       from response in _httpClient.GetResponse(request)
                       select _translator.AddTags(contactProfile, response)
                   )
                .Log(_logger, string.Format("EnrichTags({0})", contactProfile.FullName))
                .Take(1);
        }

        private static HttpRequestParameters CreateRequestParams(IPersonalIdentifier personalIdentifier, string accessToken)
        {
            var query = personalIdentifier.Value ?? string.Empty;

            var param = new HttpRequestParameters(@"https://www.google.com/m8/feeds/contacts/default/full");
            param.QueryStringParameters.Add("access_token", accessToken);
            param.QueryStringParameters.Add("q", query);
            param.Headers.Add("GData-Version", "3.0");

            return param;
        }

        private static HttpRequestParameters CreateConactGroupRequestParams(string accessToken)
        {
            var param = new HttpRequestParameters(@"https://www.google.com/m8/feeds/groups/default/full");
            param.QueryStringParameters.Add("access_token", accessToken);
            param.Headers.Add("GData-Version", "3.0");

            return param;
        }
    }
}

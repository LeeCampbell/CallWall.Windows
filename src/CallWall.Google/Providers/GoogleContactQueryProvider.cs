using System;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.Google.Authorization;
using CallWall.Web;

namespace CallWall.Google.Providers
{
    public sealed class GoogleContactQueryProvider : IContactQueryProvider
    {
        private readonly IGoogleAuthorization _authorizationModel;
        private readonly IHttpClient _webRequstService;
        private readonly ILogger _logger;

        public GoogleContactQueryProvider(IGoogleAuthorization authorizationModel, IHttpClient webRequstService, ILoggerFactory loggerFactory)
        {
            _authorizationModel = authorizationModel;
            _webRequstService = webRequstService;
            _logger = loggerFactory.CreateLogger();
        }

        public IObservable<IContactProfile> LoadContact(IProfile activeProfile)
        {
            return (
                       from accessToken in _authorizationModel.RequestAccessToken()
                           .Log(_logger, "GCQPRequestAccessToken")
                       from request in Observable.Return(CreateRequestParams(activeProfile, accessToken))
                           .Log(_logger, "ContactRequestParams")
                       from response in _webRequstService.GetResponse(request)
                       select GoogleContactProfile.Translate(response, accessToken)
                   )
                .Take(1);
        }

        private static HttpRequestParameters CreateRequestParams(IProfile activeProfile, string accessToken)
        {
            //var query = string.Join(" ", activeProfile.Identifiers.Select(i => i.Value));
            //HACK: Only searching on single Identifier at the moment.
            var query = activeProfile.Identifiers.Select(i => i.Value).FirstOrDefault() ?? string.Empty;

            var param = new HttpRequestParameters(@"https://www.google.com/m8/feeds/contacts/default/full");
            param.QueryStringParameters.Add("access_token", accessToken);
            param.QueryStringParameters.Add("q", query);
            param.Headers.Add("GData-Version", "3.0");
            return param;
        }
    }
}

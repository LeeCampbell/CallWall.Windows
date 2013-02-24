using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Web;
using Newtonsoft.Json.Linq;

namespace CallWall.Google.Authorization
{
    public sealed class GoogleOAuthService : IGoogleOAuthService
    {
        private const string ClientId = "410654176090.apps.googleusercontent.com";  //}
        private const string ClientSecret = "bDkwW8Y2RnUt0JsjbAwYA8cb";             //} TODO:This is all the Spike stuff. Might need to change.
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";             //} I probably will eventually change this to be related to a CallWall.com email/google address.

        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        public GoogleOAuthService(IHttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger();
        }


        public Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["response_type"] = "code";
            queryString["client_id"] = ClientId; //Lee.Ryan.Campbell@gmail.com client Id
            queryString["redirect_uri"] = RedirectUri;
            var scopes = requestedResources.Select(uri => uri.ToString()).ToArray();
            queryString["scope"] = string.Join(" ", scopes);    //" " should be translated into a "+" as per https://developers.google.com/accounts/docs/OAuth2InstalledApp

            var authorizationUri = new UriBuilder(@"https://accounts.google.com/o/oauth2/auth");
            authorizationUri.Query = queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
            return authorizationUri.Uri;
        }

        public IObservable<ISession> RequestAccessToken(string authorizationCode)
        {
            return Observable.Create<Session>(
                o =>
                    {
                        try
                        {
                            var request = CreateAccessTokenWebRequest(authorizationCode);
                            var requestedAt = DateTimeOffset.Now;
                            return _httpClient.GetResponse(request)
                                .Select(JObject.Parse)
                                .Select(json => new Session(
                                                    (string)json["access_token"],
                                                    (string)json["refresh_token"],
                                                    TimeSpan.FromSeconds((int)json["expires_in"]),
                                                    requestedAt))
                                .Subscribe(o);
                        }
                        catch (Exception e)
                        {
                            o.OnError(e);
                            return Disposable.Empty;
                        }
                    })
                .Log(_logger, string.Format("requestAccessToken({0})", authorizationCode));
        }

        public IObservable<ISession> RequestRefreshedAccessToken(string refreshToken)
        {
            return Observable.Create<Session>(
                o =>
                    {
                        try
                        {
                            var request = CreateRefreshTokenWebRequest(refreshToken);
                            var requestedAt = DateTimeOffset.Now;

                            return _httpClient.GetResponse(request)
                                .Select(JObject.Parse)
                                .Select(payload => new Session(
                                                       (string)payload["access_token"],
                                                       refreshToken,
                                                       TimeSpan.FromSeconds((int)payload["expires_in"]),
                                                       requestedAt))
                                .Subscribe(o);
                        }
                        catch (Exception e)
                        {
                            o.OnError(e);
                            return Disposable.Empty;
                        }
                    })
                .Log(_logger, string.Format("requestRefreshedAccessToken({0})", refreshToken));
        }


        private static HttpRequestParameters CreateAccessTokenWebRequest(string authorizationCode)
        {
            var requestParams = new HttpRequestParameters(@"https://accounts.google.com/o/oauth2/token");

            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("redirect_uri", RedirectUri);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("grant_type", "authorization_code");
            requestParams.PostParameters.Add("code", authorizationCode);

            return requestParams;
        }

        private static HttpRequestParameters CreateRefreshTokenWebRequest(string refreshToken)
        {
            var requestParams = new HttpRequestParameters(@"https://accounts.google.com/o/oauth2/token");
            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("grant_type", "refresh_token");
            requestParams.PostParameters.Add("refresh_token", refreshToken);

            return requestParams;
        }
    }
}
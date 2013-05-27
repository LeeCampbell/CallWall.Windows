using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Web;
using Newtonsoft.Json.Linq;

namespace CallWall.Google.Authorization
{
    public sealed class GoogleOAuthService : IGoogleOAuthService
    {
        private readonly IHttpClient _httpClient;
        private readonly IOAuthUriFactory _oAuthUriFactory;
        private readonly ILogger _logger;

        public GoogleOAuthService(IHttpClient httpClient, ILoggerFactory loggerFactory, IOAuthUriFactory oAuthUriFactory)
        {
            _httpClient = httpClient;
            _oAuthUriFactory = oAuthUriFactory;
            _logger = loggerFactory.CreateLogger();
        }

        public Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources)
        {
            return _oAuthUriFactory.BuildAuthorizationUri(requestedResources);
        }

        public IObservable<ISession> RequestAccessToken(string authorizationCode)
        {
            return Observable.Create<ISession>(
                o =>
                {
                    try
                    {
                        var request = _oAuthUriFactory.CreateAccessTokenWebRequest(authorizationCode);
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
            return Observable.Create<ISession>(
                o =>
                {
                    try
                    {
                        var request = _oAuthUriFactory.CreateRefreshTokenWebRequest(refreshToken);
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
    }
}
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Web;

namespace CallWall.Google.Authorization
{
    public sealed class GoogleOAuthService : IGoogleOAuthService
    {
        private readonly IHttpClient _httpClient;
        private readonly IOAuthUriFactory _oAuthUriFactory;
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger _logger;

        public GoogleOAuthService(IHttpClient httpClient, ILoggerFactory loggerFactory, IOAuthUriFactory oAuthUriFactory, ISessionFactory sessionFactory)
        {
            _httpClient = httpClient;
            _oAuthUriFactory = oAuthUriFactory;
            _sessionFactory = sessionFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources)
        {
            return _oAuthUriFactory.BuildAuthorizationUri(requestedResources);
        }

        public IObservable<ISession> RequestAccessToken(string authorizationCode, IEnumerable<Uri> requestedResources)
        {
            return Observable.Create<ISession>(
                o =>
                {
                    try
                    {
                        var request = _oAuthUriFactory.CreateAccessTokenWebRequest(authorizationCode);
                        var requestedAt = DateTimeOffset.Now;
                        return _httpClient.GetResponse(request)
                                          .Select(response => _sessionFactory.Create(response, requestedAt, requestedResources))
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

        public IObservable<ISession> RequestRefreshedAccessToken(string refreshToken, IEnumerable<Uri> authorizedResources)
        {
            return Observable.Create<ISession>(
                o =>
                {
                    try
                    {
                        var request = _oAuthUriFactory.CreateRefreshTokenWebRequest(refreshToken);
                        var requestedAt = DateTimeOffset.Now;
                        return _httpClient.GetResponse(request)
                                          .Select(response => _sessionFactory.Create(response, requestedAt, refreshToken, authorizedResources))
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
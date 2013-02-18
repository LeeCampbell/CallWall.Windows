using CallWall.Google.AccountConfiguration;
using CallWall.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CallWall.Google.Authorization
{
    //TODO: Should set to not-Authorized when the Enabled/Selected Resources differs from the ones that were actually authorized.
    //TODO: Bug when hit Authorize, then cancel/close the browser. Hitting Authorize again, does nothing.
    //TODO: Bug that only one resource is requested
    //TODO: Appears to unnecessarily refresh the access token. Note this log snippet where we refresh 4 distinct times 3, 8 & 13 seconds apart (clearly not enough time for the token to lapse).
    /*	
    158	2013-02-16 10:13:23,815 [12 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZRXmyBLEN4WQnhwOZxYPgka4xLgfN-N9J0tavEXCpk', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:22.8208044+00:00'})
	232	2013-02-16 10:13:26,422 [39 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZRJVLJBz6dMrVAwhv_chwBCiPxJ9NRxpzg7Xm3Ci1M', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:24.0174898+00:00'})
	282	2013-02-16 10:13:27,026 [14 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZRJVLJBz6dMrVAwhv_chwBCiPxJ9NRxpzg7Xm3Ci1M', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:24.6209151+00:00'})
	387	2013-02-16 10:13:34,852 [37 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZT8-C62c-wpiaWDHOnoqjMyqvrF-3Y-d7qk2Qbwj1o', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:32.6475378+00:00'})
	388	2013-02-16 10:13:35,050 [12 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZT8-C62c-wpiaWDHOnoqjMyqvrF-3Y-d7qk2Qbwj1o', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:31.6394237+00:00'})
	551	2013-02-16 10:13:47,720 [40 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZSTJNul6ZJ8MabdWQPSOkEdobUTaPIdeJnTe4fH1z0', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:40.0697386+00:00'})
	557	2013-02-16 10:13:50,528 [12 ] TRACE GoogleAuthorization - refreshSession().OnNext(Session { AccessToken : 'ya29.AHES6ZSTJNul6ZJ8MabdWQPSOkEdobUTaPIdeJnTe4fH1z0', RefreshToken : '1/vR5Ql9nq23cmJo_u-wQMNudYoSlDkixf1y-evmsTXc8', Expires : '2013-02-16T11:13:40.6731207+00:00'})
    */
    public sealed class GoogleAuthorization : IGoogleAuthorization
    {
        private const string ClientId = "410654176090.apps.googleusercontent.com";  //}
        private const string ClientSecret = "bDkwW8Y2RnUt0JsjbAwYA8cb";             //} TODO:This is all the Spike stuff. Might need to change.
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";             //} I probably will eventually change this to be related to a CallWall.com email/google address.

        private readonly IPersonalizationSettings _localStore;
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ReadOnlyCollection<GoogleResource> _availableResourceScopes;
        private readonly BehaviorSubject<AuthorizationStatus> _status = new BehaviorSubject<AuthorizationStatus>(AuthorizationStatus.NotAuthorized);

        private RequestAuthorizationCode _callback;
        private Session _currentSession;

        public GoogleAuthorization(IPersonalizationSettings localStore, IHttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _localStore = localStore;
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger();
            _availableResourceScopes = GoogleResource.AvailableResourceScopes();
            _currentSession = LoadSession();
            if (_currentSession != null)
            {
                _status.OnNext(AuthorizationStatus.Authorized);
            }
        }

        public IObservable<AuthorizationStatus> Status
        {
            get { return _status.AsObservable(); }
        }

        public ReadOnlyCollection<GoogleResource> AvailableResourceScopes { get { return _availableResourceScopes; } }

        private string AuthorizationCode
        {
            get { return _localStore.Get("Google.AuthorizationCode"); }
            set { _localStore.Put("Google.AuthorizationCode", value); }
        }

        private Session CurrentSession
        {
            get { return _currentSession; }
            set
            {
                if (_currentSession == value) 
                    return;

                _logger.Verbose("Setting accessToken from {0} to {1}",
                    _currentSession == null ? "null" : _currentSession.AccessToken,
                    value == null ? "null" : value.AccessToken);

                _currentSession = value;

                if (_currentSession == null)
                {
                    _localStore.Remove("Google.AccessToken");
                    _localStore.Remove("Google.AccessTokenExpires");
                    _localStore.Remove("Google.RefreshToken");
                }
                else
                {
                    _localStore.Put("Google.AccessToken", _currentSession.AccessToken);
                    _localStore.Put("Google.AccessTokenExpires", _currentSession.Expires.ToString("o"));
                    _localStore.Put("Google.RefreshToken", _currentSession.RefreshToken);
                    _status.OnNext(AuthorizationStatus.Authorized);
                }
            }
        }


        public void RegisterAuthorizationCallback(RequestAuthorizationCode callback)
        {
            _callback = callback;
        }

        public IObservable<string> RequestAccessToken()
        {
            var requestedResources = AvailableResourceScopes
                .Where(rs => rs.IsEnabled)
                .Select(s => s.Resource)
                .ToArray();

            if (requestedResources.Length == 0)
                return Observable.Throw<string>(new InvalidOperationException("No resources have been enabled."));

            var currentSession = Observable.Return(CurrentSession);
            var refreshSession = Observable.Defer(() => RefreshSession(requestedResources));
            var createSession = Observable.Defer(() => CreateSession(requestedResources));

            var sessionPriorities = ObservableExtensions.LazyConcat(currentSession, refreshSession, createSession);

            var sequence = sessionPriorities
                .Where(session => session != null && !session.HasExpired())
                .Do(session => CurrentSession = session)
                .Take(1)
                .Select(session => session.AccessToken);
            return sequence.Log(_logger, "RequestAccessToken()");
        }


        private Session LoadSession()
        {
            var accessToken = _localStore.Get("Google.AccessToken");
            var strExpires = _localStore.Get("Google.AccessTokenExpires");
            var refreshToken = _localStore.Get("Google.RefreshToken");
            if (accessToken == null || strExpires == null || refreshToken == null)
                return null;
            var expires = DateTimeOffset.Parse(strExpires);
            return new Session(accessToken, refreshToken, expires);
        }

        private IObservable<string> GetAuthorizationCode(Uri[] requestedResources)
        {
            return Observable.Create<string>(
                o =>
                {
                    if (AuthorizationCode != null)
                    {
                        return Observable.Return(AuthorizationCode)
                            .Subscribe(o);
                    }
                    return RequestAuthorizationCode(requestedResources)
                        .Do(newCode => AuthorizationCode = newCode)
                        .Subscribe(o);
                })
                .Log(_logger, "getAuthorizationCode()");
        }

        private IObservable<string> RequestAuthorizationCode(Uri[] requestedResources)
        {
            return Observable.Create<string>(
                o =>
                {
                    if (_callback == null)
                        throw new InvalidOperationException("No call-back has been registered via the RegisterAuthorizationCallback method");
                    var uri = BuildAuthorizationUri(requestedResources);
                    return _callback(uri).Subscribe(o);
                })
                .Log(_logger, "requestAuthorizationCode()");
        }

        private IObservable<Session> CreateSession(Uri[] requestedResources)
        {
            return (from authCode in GetAuthorizationCode(requestedResources)
                    from accessToken in RequestAccessToken(authCode)
                    select accessToken).Log(_logger, "createSession()");
        }

        private IObservable<Session> RefreshSession(Uri[] requestedResources)
        {
            if (CurrentSession == null)
                return Observable.Empty<Session>().Log(_logger, "refreshSession()");
            return (from authCode in GetAuthorizationCode(requestedResources)
                    from accessToken in RequestRefreshedAccessToken(CurrentSession.RefreshToken)
                    select accessToken).Log(_logger, "refreshSession()");
        }

        private IObservable<Session> RequestAccessToken(string authorizationCode)
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

        private IObservable<Session> RequestRefreshedAccessToken(string refreshToken)
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
            requestParams.PostParameters.Add("code", authorizationCode);
            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("redirect_uri", RedirectUri);
            requestParams.PostParameters.Add("grant_type", "authorization_code");
            return requestParams;
        }

        private static HttpRequestParameters CreateRefreshTokenWebRequest(string refreshToken)
        {
            var requestParams = new HttpRequestParameters(@"https://accounts.google.com/o/oauth2/token");
            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("refresh_token", refreshToken);
            requestParams.PostParameters.Add("grant_type", "refresh_token");
            return requestParams;
        }

        private static Uri BuildAuthorizationUri(Uri[] requestedResources)
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
    }
}
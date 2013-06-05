using CallWall.Google.AccountConfiguration;
using CallWall.Google.Properties;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

//Was fun getting kicked around by the tesco interview today. This class clearly does way too much. Time to carve stuff off it. -LC
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
        //TODO: I need to ensure that I am setting IsProcessing appropriately. -LC


        private readonly IPersonalizationSettings _localStore;
        private readonly IGoogleOAuthService _oAuthService;
        private readonly ILogger _logger;
        //private readonly HashSet<Uri> _authorizedResources = new HashSet<Uri>();
        private AuthorizationStatus _status = AuthorizationStatus.Uninitialized;

        private RequestAuthorizationCode _callback;
        private ISession _currentSession;

        public GoogleAuthorization(IPersonalizationSettings localStore, IGoogleOAuthService oAuthService, ILoggerFactory loggerFactory)
        {
            _localStore = localStore;
            _oAuthService = oAuthService;
            _logger = loggerFactory.CreateLogger();
            _currentSession = LoadSession();
            if (_currentSession != null)
            {
                Status = AuthorizationStatus.Authorized(_currentSession.AuthorizedResources);
            }
        }

        public AuthorizationStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public ReadOnlyCollection<GoogleResource> AvailableResourceScopes { get { return GoogleResource.AvailableResourceScopes; } }

        private string AuthorizationCode
        {
            get { return _localStore.Get(LocalStoreKeys.AuthorizationCode); }
            set { _localStore.Put(LocalStoreKeys.AuthorizationCode, value); }
        }

        private ISession CurrentSession
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
                    _localStore.Remove(LocalStoreKeys.AccessToken);
                    _localStore.Remove(LocalStoreKeys.AccessTokenExpires);
                    _localStore.Remove(LocalStoreKeys.RefreshToken);
                    _localStore.Remove(LocalStoreKeys.AuthorizedResources);

                    Status = AuthorizationStatus.Error(Resources.Authorization_FailedToAuthorize);
                }
                else
                {
                    _localStore.Put(LocalStoreKeys.AccessToken, _currentSession.AccessToken);
                    _localStore.Put(LocalStoreKeys.AccessTokenExpires, _currentSession.Expires.ToString("o"));
                    _localStore.Put(LocalStoreKeys.RefreshToken, _currentSession.RefreshToken);
                    _localStore.Put(LocalStoreKeys.AuthorizedResources, string.Join(";", _currentSession.AuthorizedResources));

                    Status = AuthorizationStatus.Authorized(_currentSession.AuthorizedResources);
                }
            }
        }


        public void RegisterAuthorizationCallback(RequestAuthorizationCode callback)
        {
            _callback = callback;
            if (!Status.IsInitialized)
            {
                Status = AuthorizationStatus.NotAuthorized;
            }
        }

        public IObservable<Unit> Authorize(IEnumerable<GoogleResource> resources)
        {
            return Observable.Create<Unit>(
                o =>
                {
                    var requestedResources = resources.Select(r => r.Resource).ToArray();
                    if (Status.IsAuthorized && CurrentSession != null && CurrentSession.AuthorizedResources.SetEquals(requestedResources))
                        return Observable.Return(Unit.Default).Subscribe(o);
                    if (!Status.IsInitialized)
                    {
                        o.OnError(new InvalidOperationException(Status.ErrorMessage));
                        return Disposable.Empty;
                    }
                    Status = AuthorizationStatus.Processing;

                    if (!requestedResources.Any())
                    {

                        Status = AuthorizationStatus.Error(Resources.Authorization_NoResourcesSelected);
                        o.OnError(new InvalidOperationException(Resources.Authorization_NoResourcesSelected));
                        return Disposable.Empty;
                    }


                    return CreateSession(requestedResources)
                        .Concat(Observable.Return<ISession>(null))
                        .Take(1)
                        .Subscribe(
                            session =>
                            {
                                CurrentSession = session;
                                if (Status.IsProcessing)
                                    Status = AuthorizationStatus.NotAuthorized;
                                o.OnCompleted();
                            },
                            ex => { Status = AuthorizationStatus.NotAuthorized; o.OnError(ex); });
                });
        }

        public IObservable<string> RequestAccessToken(GoogleResource resource)
        {
            return Observable.Create<string>(
                o =>
                {
                    if (!Status.IsAuthorized)
                    {
                        o.OnError(new InvalidOperationException("Can not request access token until authorized."));
                        return Disposable.Empty;
                    }
                    if (!CurrentSession.AuthorizedResources.Contains(resource.Resource))
                        return Observable.Empty<string>().Subscribe(o);

                    var currentSession = Observable.Return(CurrentSession);
                    var refreshSession = Observable.Defer(RefreshSession);

                    var sessionPriorities = ObservableExtensions.LazyConcat(currentSession, refreshSession);

                    var sequence = sessionPriorities
                        .Where(session => session != null && !session.HasExpired())
                        .Do(session => CurrentSession = session)
                        .Take(1)
                        .Select(session => session.AccessToken);
                    return sequence.Log(_logger, "RequestAccessToken()")
                                   .Subscribe(o);
                });
        }


        private Session LoadSession()
        {
            var accessToken = _localStore.Get(LocalStoreKeys.AccessToken);
            var strExpires = _localStore.Get(LocalStoreKeys.AccessTokenExpires);
            var refreshToken = _localStore.Get(LocalStoreKeys.RefreshToken);
            if (accessToken == null || strExpires == null || refreshToken == null)
                return null;
            var expires = DateTimeOffset.Parse(strExpires);

            var lastSessionResources = (_localStore.Get(LocalStoreKeys.AuthorizedResources) ?? string.Empty)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(path => new Uri(path));

            return new Session(accessToken, refreshToken, expires, lastSessionResources);
        }

        private IObservable<ISession> CreateSession(IEnumerable<Uri> requestedResources)
        {
            var resources = requestedResources.ToArray();
            return (from authCode in GetAuthorizationCode(resources)
                    from accessToken in _oAuthService.RequestAccessToken(authCode, resources)
                    select accessToken).Log(_logger, "createSession()");
        }

        private IObservable<string> GetAuthorizationCode(IEnumerable<Uri> requestedResources)
        {
            return Observable.Create<string>(
                o =>
                {
                    //If we have an Authorization code from a previous session, we can continue to use that.
                    //TODO:What if the AuthCode has since been rejected? -LC
                    //TODO: What if the requestedResouces dont match the ones from the stored AuthCode -LC (if we store them, then order them, lower case them and comma delimit them for fast comparison)
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

        private IObservable<string> RequestAuthorizationCode(IEnumerable<Uri> requestedResources)
        {
            return Observable.Create<string>(
                o =>
                {
                    if (_callback == null)
                        throw new InvalidOperationException("No call-back has been registered via the RegisterAuthorizationCallback method");
                    var uri = _oAuthService.BuildAuthorizationUri(requestedResources);
                    return _callback(uri).Subscribe(o);
                })
                .Log(_logger, "requestAuthorizationCode()");
        }

        private IObservable<ISession> RefreshSession()
        {
            if (CurrentSession == null)
                return Observable.Empty<Session>().Log(_logger, "refreshSession()");
            return (from newSession in _oAuthService.RequestRefreshedAccessToken(CurrentSession.RefreshToken, CurrentSession.AuthorizedResources)
                    select newSession).Log(_logger, "refreshSession()");
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
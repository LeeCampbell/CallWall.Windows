using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Google.Authorization.Login
{
    public sealed class LoginController : ILoginController
    {
        private readonly IRegionManager _regionManager;
        private readonly IGoogleAuthorization _googleAuthorization;
        private readonly IGoogleLoginView _loginView;
        private readonly ILogger _logger;

        public LoginController(IRegionManager regionManager, 
            IGoogleAuthorization googleAuthorization, 
            IGoogleLoginView loginView, 
            ILoggerFactory loggerFactory)
        {
            _regionManager = regionManager;
            _googleAuthorization = googleAuthorization;
            _loginView = loginView;
            _logger = loggerFactory.CreateLogger();
        }

        public void Start()
        {
            _regionManager.Regions[RegionNames.PopupWindowRegion].Add(_loginView);
            _googleAuthorization.RegisterAuthorizationCallback(ShowGoogleLogin);
        }

        private IObservable<string> ShowGoogleLogin(Uri authorizationUri)
        {
            return Observable.Create<string>(
                o =>
                {
                    _loginView.ViewModel.AuthorizationUri = authorizationUri;
                    _regionManager.Regions[RegionNames.PopupWindowRegion].Activate(_loginView);

                    var isActiveChanged = Observable.FromEventPattern<EventHandler, EventArgs>(
                        h => _loginView.IsActiveChanged += h,
                        h => _loginView.IsActiveChanged -= h);

                    var authorizationCodeChanged = _loginView.ViewModel
                        .WhenPropertyChanges(vm => vm.AuthorizationCode)
                        .TakeUntil(isActiveChanged.Where(_ => !_loginView.IsActive))
                        .Take(1)
                        .Log(_logger, "ShowGoogleLogin");

                    return Observable.Using(
                        () => Disposable.Create(() => _regionManager.Regions[RegionNames.PopupWindowRegion].Deactivate(_loginView)),
                        _ => authorizationCodeChanged)
                        .Subscribe(o);
                });
        }
    }
}
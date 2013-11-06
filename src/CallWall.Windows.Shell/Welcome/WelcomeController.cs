using CallWall.Windows.Shell.Settings.Accounts;
using CallWall.Windows.Shell.Settings.Connectivity;
using CallWall.Windows.Shell.Settings.Demonstration;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Windows.Shell.Welcome
{
    public sealed class WelcomeController : IWelcomeController
    {
        private readonly IRegionManager _regionManager;
        private readonly IWelcomeView _welcomeView;
        private readonly IWelcomeStep1View _welcomeStep1View;
        private readonly IConnectivitySettingsView _connectivitySettingsView;
        private readonly IAccountSettingsView _accountSettingsView;
        private readonly IDemoView _demoView;

        public WelcomeController(IRegionManager regionManager,
            IWelcomeView welcomeView,
            IWelcomeStep1View welcomeStep1View,
            IConnectivitySettingsView connectivitySettingsView,
            IAccountSettingsView accountSettingsView,
            IDemoView demoView)
        {
            _regionManager = regionManager;
            _welcomeView = welcomeView;
            _welcomeStep1View = welcomeStep1View;
            _connectivitySettingsView = connectivitySettingsView;
            _accountSettingsView = accountSettingsView;
            _demoView = demoView;
        }

        public void Start()
        {
            if (!_accountSettingsView.ViewModel.RequiresSetup && !_connectivitySettingsView.ViewModel.RequiresSetup)
                return;

            //The Test screen will explain that for a test we will emulate what a call from your own number would be like.
            //Invoke the Incoming event with the current user' data

            _welcomeView.ViewModel.CloseCommand =
                new DelegateCommand(() => _regionManager.Regions[RegionNames.WindowRegion].Remove(_welcomeView));
            _regionManager.AddToRegion(RegionNames.WindowRegion, _welcomeView);

            var welcomeSettingRegion = _regionManager.Regions[ShellRegionNames.WelcomeSettingsRegion];
            welcomeSettingRegion.Add(_welcomeStep1View);
            welcomeSettingRegion.Add(_connectivitySettingsView);
            welcomeSettingRegion.Add(_accountSettingsView);
            welcomeSettingRegion.Add(_demoView);

            _regionManager.Regions[RegionNames.WindowRegion].Activate(_welcomeView);
            welcomeSettingRegion.Activate(_welcomeStep1View);

            _welcomeStep1View.NextView += (s, e) => welcomeSettingRegion.Activate(_connectivitySettingsView);
            _connectivitySettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_accountSettingsView);
            _accountSettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_demoView);
        }
    }
}
using CallWall.Settings.Connectivity;
using CallWall.Settings.Providers;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Welcome
{
    public sealed class WelcomeController : IWelcomeController
    {
        private readonly IRegionManager _regionManager;
        private readonly IWelcomeView _welcomeView;
        private readonly IWelcomeStep1View _welcomeStep1View;
        private readonly IConnectivitySettingsView _connectivitySettingsView;
        private readonly IProviderSettingsView _providerSettingsView;
        private readonly IDemoView _demoView;

        public WelcomeController(IRegionManager regionManager, 
            IWelcomeView welcomeView, 
            IWelcomeStep1View welcomeStep1View, 
            IConnectivitySettingsView connectivitySettingsView, 
            IProviderSettingsView providerSettingsView, 
            IDemoView demoView)
        {
            _regionManager = regionManager;
            _welcomeView = welcomeView;
            _welcomeStep1View = welcomeStep1View;
            _connectivitySettingsView = connectivitySettingsView;
            _providerSettingsView = providerSettingsView;
            _demoView = demoView;
        }

        public void Start()
        {
            if (!_providerSettingsView.ViewModel.RequiresSetup && !_connectivitySettingsView.ViewModel.RequiresSetup)
                return;

            //The Test screen will explain that for a test we will emulate what a call from your own number would be like.
            //Invoke the Incoming event with the current user' data

            _regionManager.AddToRegion(RegionNames.Modal, _welcomeView);

            var welcomeSettingRegion = _regionManager.Regions[ShellRegionNames.WelcomeSettingsRegion];
            welcomeSettingRegion.Add(_welcomeStep1View);
            welcomeSettingRegion.Add(_connectivitySettingsView);
            welcomeSettingRegion.Add(_providerSettingsView);
            welcomeSettingRegion.Add(_demoView);

            welcomeSettingRegion.Activate(_welcomeStep1View);

            _welcomeStep1View.NextView+= (s, e) => welcomeSettingRegion.Activate(_connectivitySettingsView);
            _connectivitySettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_providerSettingsView);
            _providerSettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_demoView);
        }
    }
}
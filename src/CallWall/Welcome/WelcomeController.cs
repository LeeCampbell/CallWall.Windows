using CallWall.Settings.Connectivity;
using CallWall.Settings.Providers;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Welcome
{
    public class WelcomeController : IWelcomeController
    {
        private readonly IRegionManager _regionManager;
        private readonly IWelcomeView _welcomeView;
        private readonly IConnectivitySettingsView _connectivitySettingsView;
        private readonly IProviderSettingsView _providerSettingsView;

        public WelcomeController(IRegionManager regionManager, 
                                 IWelcomeView welcomeView,
                                 IConnectivitySettingsView connectivitySettingsView,
                                 IProviderSettingsView providerSettingsView)
        {
            _regionManager = regionManager;
            _welcomeView = welcomeView;
            _connectivitySettingsView = connectivitySettingsView;
            _providerSettingsView = providerSettingsView;
        }

        #region Implementation of IWelcomeController

        public void Start()
        {
            //TODO: If no providers or connections are set up, then show the Welcome screen.
            if(_providerSettingsView.ViewModel.RequiresSetup || _connectivitySettingsView.ViewModel.RequiresSetup)
            {
                //Create a sub region manager
                //Load the IConnectivitySettingsView and the IProviderSettingsView into the Step Region (ItemsControl that is the accordion?)
                //When the I*SettingsView.Done/Closed/Exit event is raised, then move to the next view. If the last view, then show the test screen.
                //The Test screen will explain that for a test we will emulate what a call from your own number would be like.
                //Invoke the Incoming event with the current user' data
                
                _regionManager.AddToRegion("ModalRegion", _welcomeView);    
            }
        }

        #endregion
    }
}
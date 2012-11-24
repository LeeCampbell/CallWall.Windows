using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace CallWall.Welcome
{
    public sealed class WelcomeModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public WelcomeModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //TODO: Register my instances and then run the welcome screen. It will only show if required.
            //_container.RegisterType<IType, Type>(new ContainerControlledLifetimeManager());

            //TODO: If the user has dismissed the welcome screen, then do not show it.
            //TODO: If no providers or connections are set up, then show the Welcome screen.
            var welcomeView = _container.Resolve<WelcomeView>();
            _regionManager.AddToRegion("ModalRegion", welcomeView);

            //Create a sub region manger
            //Load the IConnectivitySettingsView and the IProviderSettingsView into the Step Region (ItemsControl that is the accordion?)
            //When the I*SettingsView.Done/Closed/Exit event is raised, then move to the next view. If the last view, then show the test screen.
            //The Test screen will explain that for a test we will emulate what a call from your own number would be like.
            //Invoke the Incoming event with the current user' data

        }
    }
}

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

        }
    }
}

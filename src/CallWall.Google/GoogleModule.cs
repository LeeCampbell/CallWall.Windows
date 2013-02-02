using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Google
{
    public sealed class GoogleModule :IModule
    {
        private readonly IUnityContainer _container;

        public GoogleModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.GoogleAccountConfiguration>("GoogleAccountConfiguration", new ContainerControlledLifetimeManager());
            
            _container.RegisterType<AccountConfiguration.IGoogleAccountSetupView, AccountConfiguration.GoogleAccountSetupView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<AccountConfiguration.IGoogleAccountSetup, AccountConfiguration.GoogleAccountSetup>(new ContainerControlledLifetimeManager());

            //Contacts
            //Mail
            //Talk
            //Images??
            //Calendar
        }
    }
}

using CallWall.Contract.Communication;
using CallWall.Contract.Contact;
using CallWall.Contract.Picture;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.FakeProvider
{
    public sealed class FakeModule : IModule
    {
        private readonly IUnityContainer _container;

        public FakeModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IConnectionConfiguration, Connectivity.UsbConnectionConfiguration>("UsbConnectionConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IConnectionConfiguration, Connectivity.WifiDirectConnectionConfiguration>("WifiDirectConnectionConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IConnectionConfiguration, Connectivity.CloudConnectionConfiguration>("CloudConnectionConfiguration", new ContainerControlledLifetimeManager());

            _container.RegisterType<IAccountConfiguration, AccountConfiguration.MicrosoftAccountConfiguration>("MicrosoftAccountConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.FacebookAccountConfiguration>("FacebookAccountConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.LinkedInAccountConfiguration>("LinkedInAccountConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.TwitterAccountConfiguration>("TwitterAccountConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.YahooAccountConfiguration>("YahooAccountConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.GithubAccountConfiguration>("GithubAccountConfiguration", new ContainerControlledLifetimeManager());
            
            _container.RegisterType<IContactQueryProvider, Providers.FakeGoogleContactQueryProvider>("FakeGoogleContactQueryProvider", new ContainerControlledLifetimeManager());

            _container.RegisterType<ICommunicationQueryProvider, Providers.GmailCommunicationQueryProvider>("FakeGmailCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommunicationQueryProvider, Providers.SmsCommunicationQueryProvider>("FakeSmsCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommunicationQueryProvider, Providers.TwitterCommunicationQueryProvider>("FakeTwitterCommunicationQueryProvider", new ContainerControlledLifetimeManager());

            _container.RegisterType<IPictureQueryProvider, Providers.SkydrivePictureQueryProvider>("SkydrivePictureQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<IPictureQueryProvider, Providers.FacebookPictureQueryProvider>("FacebookPictureQueryProvider", new ContainerControlledLifetimeManager());
        }
    }
}

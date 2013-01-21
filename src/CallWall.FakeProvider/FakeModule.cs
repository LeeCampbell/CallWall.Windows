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
            
            _container.RegisterType<IContactQueryProvider, Providers.FakeGoogleContactQueryProvider>("FakeGoogleContactQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommunicationQueryProvider, Providers.GmailCommunicationQueryProvider>("GmailCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommunicationQueryProvider, Providers.SmsCommunicationQueryProvider>("SmsCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommunicationQueryProvider, Providers.TwitterCommunicationQueryProvider>("TwitterCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<IPictureQueryProvider, Providers.SkydrivePictureQueryProvider>("SkydrivePictureQueryProvider", new ContainerControlledLifetimeManager());
            

            //_container.RegisterType<IIdentityActivator, UsbIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, BluetoothIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, WifiDirectIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, CloutIdentityActivator>(new TransientLifetimeManager());
        }
    }
}

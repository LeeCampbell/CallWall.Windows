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

            //_container.RegisterType<IIdentityActivator, UsbIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, BluetoothIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, WifiDirectIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, CloutIdentityActivator>(new TransientLifetimeManager());
        }
    }
}

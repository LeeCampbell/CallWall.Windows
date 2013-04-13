using CallWall.Activators;
using CallWall.PrismExtensions;
using CallWall.ProfileDashboard;
using CallWall.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Shell
{
    public sealed class ShellModule : IModule
    {
        private readonly IUnityContainer _container;

        public ShellModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            //_container.RegisterType<IProfileActivator, FakeProfileActivator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IBluetoothService, BluetoothService>(new ContainerControlledLifetimeManager());
            _container.RegisterComposite<IProfileActivator, IBluetoothProfileActivator, BluetoothProfileActivator>();
            //_container.RegisterComposite<IProfileActivator, IUsbIdentityActivator, UsbIdentityActivator>();
            //_container.RegisterComposite<IProfileActivator, IWifiDirectIdentityActivator, WifiDirectIdentityActivator>();
            //_container.RegisterComposite<IProfileActivator, ICloudIdentityActivator, CloudIdentityActivator>();
            //_container.RegisterComposite<IProfileActivator, IIsdnIdentityActivator, CloudIdentityActivator>();

            _container.RegisterType<IProfileActivatorAggregator, ProfileActivatorAggregator>(new ContainerControlledLifetimeManager());
        }
    }
}

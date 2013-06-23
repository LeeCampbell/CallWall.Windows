using System;
using CallWall.Activators;
using CallWall.PrismExtensions;
using CallWall.ProfileDashboard;
using CallWall.Services;
using CallWall.Toolbar;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Shell
{
    public sealed class ShellModule : IModule, IDisposable
    {
        private readonly IUnityContainer _container;
        private IToolbarController _toolbarController;

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

            _container.RegisterType<Toolbar.IToolbarController, Toolbar.ToolbarController>(new ContainerControlledLifetimeManager());
            _toolbarController = _container.Resolve<Toolbar.IToolbarController>();
            _toolbarController.Start();
        }

        public void Dispose()
        {
            //_toolbarController.Dispose();
        }
    }
}

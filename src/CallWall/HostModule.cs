using CallWall.Activators;
using CallWall.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall
{
    public sealed class HostModule : IModule
    {
        private readonly IUnityContainer _container;

        public HostModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ISchedulerProvider, SchedulerProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPersonalizationSettings, PersonalizationSettings>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IBluetoothService, BluetoothService>(new ContainerControlledLifetimeManager());

            //http://www.stackoverflow.com/questions/10910237
            _container.RegisterType<IBluetoothProfileActivator, BluetoothProfileActivator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProfileActivator, BluetoothProfileActivator>("BluetoothProfileActivator",
                new ExternallyControlledLifetimeManager(),
                new InjectionFactory(u => u.Resolve<IBluetoothProfileActivator>()));
            
            
            //_container.RegisterType<IIdentityActivator, UsbIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, BluetoothIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, WifiDirectIdentityActivator>(new TransientLifetimeManager());
            //_container.RegisterType<IIdentityActivator, CloutIdentityActivator>(new TransientLifetimeManager());
        }
    }

    //TODO: Create a WelcomeModule. Shows welcome screens and tips on how to use the application
    //TODO: Create a SettingsModule. Handles the setting of Accounts and Pairing of devices (USB, Bluetooth, WiFiDirect, Cloud)
    //TODO: Create an IncomingCallModule. The key functionality. Listens, aggregates and presents.
    //Can we load the Providers on the BG thread to make start up as fast as possible? WPF has such a slow start up time!
}
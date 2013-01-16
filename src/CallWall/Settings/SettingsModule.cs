using CallWall.Settings.Demonstration;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Settings
{
    public sealed class SettingsModule : IModule
    {
        private readonly IUnityContainer _container;

        public SettingsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            //Connectivity view
            _container.RegisterType<Connectivity.IConnectivitySettingsModel, Connectivity.ConnectivitySettingsModel>(new TransientLifetimeManager());
            _container.RegisterType<Connectivity.IConnectivitySettingsViewModel, Connectivity.ConnectivitySettingsViewModel>(new TransientLifetimeManager());
            _container.RegisterType<Connectivity.IConnectivitySettingsView, Connectivity.ConnectivitySettingsView>(new TransientLifetimeManager());

            //_container.RegisterType<IConnectionConfiguration, Settings.Usb.UsbConnectivityConfigurator>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IConnectionConfiguration, Settings.WifiDirect.WifiDirectConnectivityConfigurator>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IConnectionConfiguration, Settings.Cloud.CloudConnectivityConfigurator>(new ContainerControlledLifetimeManager());

            //  Bluetooth Connectivity
            _container.RegisterType<IConnectionConfiguration, Connectivity.Bluetooth.BluetoothConnectionConfiguration>("BluetoothConnectionConfiguration", new ContainerControlledLifetimeManager());
            _container.RegisterType<Connectivity.Bluetooth.IBluetoothSetupView, Connectivity.Bluetooth.BluetoothSetupView>(new TransientLifetimeManager());
            _container.RegisterType<Connectivity.Bluetooth.IBluetoothSetupViewModel, Connectivity.Bluetooth.BluetoothSetupViewModel>(new TransientLifetimeManager());

            //Accounts view
            _container.RegisterType<Accounts.IAccountSettingsModel, Accounts.AccountSettingsModel>(new TransientLifetimeManager());
            _container.RegisterType<Accounts.IAccountSettingsViewModel, Accounts.AccountSettingsViewModel>(new TransientLifetimeManager());
            _container.RegisterType<Accounts.IAccountSettingsView, Accounts.AccountSettingsView>(new TransientLifetimeManager());

            //DemoView
            _container.RegisterType<IDemoView, DemoView>(new TransientLifetimeManager());
            //http://www.stackoverflow.com/questions/10910237
            _container.RegisterType<IDemoActivatedIdentityListener, DemoActivatedIdentityListener>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IActivatedIdentityListener, DemoActivatedIdentityListener>("DemoActivatedIdentityListener", 
                new ExternallyControlledLifetimeManager(), 
                new InjectionFactory(u=>u.Resolve<IDemoActivatedIdentityListener>()));

            //var demoListener = new Demonstration.DemoActivatedIdentityListener();
            //_container.RegisterInstance<Demonstration.IDemoActivatedIdentityListener>(demoListener);
            //_container.RegisterInstance<IActivatedIdentityListener>(demoListener);

        }
    }


    //SettingsModel
    //  IE<IConnectionSetting> ConnectionSettings
    //  IE<IProvderSetting> AccountSettings

    //SettingsViewModel
    //  IE<SubView> ConnectionSettings
    //  IE<SubView> AccountSettings

    //SubView
    //  string Name
    //  Uri Image
    //  DelgateCommand Open

    //IConnectionSetting
    //  string Name
    //  Uri Image
    //  ..other things to actual manage the thing

    //IProvderSetting
    //  string Name
    //  Uri Image
    //  ..other things to actual manage the thing
}

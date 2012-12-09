using CallWall.Settings.Accounts;
using CallWall.Settings.Bluetooth;
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
            _container.RegisterType<Connectivity.IConnectivitySettingsModel, Connectivity.ConnectivitySettingsModel>(new TransientLifetimeManager());
            _container.RegisterType<Connectivity.IConnectivitySettingsViewModel, Connectivity.ConnectivitySettingsViewModel>(new TransientLifetimeManager());
            _container.RegisterType<Connectivity.IConnectivitySettingsView, Connectivity.ConnectivitySettingsView>(new TransientLifetimeManager());
            _container.RegisterType<IAccountSettingsModel, AccountSettingsModel>(new TransientLifetimeManager());
            _container.RegisterType<IAccountSettingsViewModel, AccountSettingsViewModel>(new TransientLifetimeManager());
            _container.RegisterType<IAccountSettingsView, AccountSettingsView>(new TransientLifetimeManager());
            _container.RegisterType<IBluetoothSetup, BluetoothSetup>(new TransientLifetimeManager());
            _container.RegisterType<IBluetoothSetupView, BluetoothSetupView>(new TransientLifetimeManager());
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

using CallWall.Windows.Contract;
using CallWall.Windows.Shell.Settings.Demonstration;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Windows.Shell.Settings
{
    public sealed class SettingsModule : IModule
    {
        private readonly ITypeRegistry _registry;

        public SettingsModule(ITypeRegistry registry)
        {
            _registry = registry;
        }

        public void Initialize()
        {
            //Connectivity view
            _registry.RegisterTypeAsTransient <Connectivity.IConnectivitySettingsModel, Connectivity.ConnectivitySettingsModel>();
            _registry.RegisterTypeAsTransient<Connectivity.IConnectivitySettingsViewModel, Connectivity.ConnectivitySettingsViewModel>();
            _registry.RegisterTypeAsTransient<Connectivity.IConnectivitySettingsView, Connectivity.ConnectivitySettingsView>();

            //Accounts view
            _registry.RegisterTypeAsTransient<Accounts.IAccountSettingsModel, Accounts.AccountSettingsModel>();
            _registry.RegisterTypeAsTransient<Accounts.IAccountSettingsViewModel, Accounts.AccountSettingsViewModel>();
            _registry.RegisterTypeAsTransient<Accounts.IAccountSettingsView, Accounts.AccountSettingsView>();

            //DemoView
            _registry.RegisterTypeAsTransient<IDemoView, DemoView>();
            _registry.RegisterCompositeAsSingleton<IProfileActivator, IDemoProfileActivator, DemoActivatedIdentityListener>();

            _registry.RegisterTypeAsSingleton<ISettingsView, SettingsView>();
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

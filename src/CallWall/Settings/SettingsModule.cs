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
            _container.RegisterType<Providers.IProviderSettingsModel, Providers.ProviderSettingsModel>(new TransientLifetimeManager());
            _container.RegisterType<Providers.IProviderSettingsViewModel, Providers.ProviderSettingsViewModel>(new TransientLifetimeManager());
            _container.RegisterType<Providers.IProviderSettingsView, Providers.ProviderSettingsView>(new TransientLifetimeManager());
        }
    }


    //SettingsModel
    //  IE<IConnectionSetting> ConnectionSettings
    //  IE<IProvderSetting> ProviderSettings

    //SettingsViewModel
    //  IE<SubView> ConnectionSettings
    //  IE<SubView> ProviderSettings

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

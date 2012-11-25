using System.Text;
using System.Threading.Tasks;
using CallWall.Welcome;
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
            _container.RegisterType<Connectivity.IConnectivitySettingsView, Connectivity.ConnectivitySettingsView>(new TransientLifetimeManager());
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

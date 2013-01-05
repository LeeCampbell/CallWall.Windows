using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Settings
{
    public sealed class SettingsViewModel
    {
        private readonly IEnumerable<SubView> _connectivityConfigurators;
        private readonly IEnumerable<SubView> _providers;

        public SettingsViewModel(IRegionManager regionManager, IEnumerable<IConnectionConfiguration> connectivityConfigurators, IEnumerable<IProvider> providers)
        {
            _connectivityConfigurators = connectivityConfigurators.Select(config => new SubView(config.Name, config.Image, () => regionManager.AddToRegion(RegionNames.Modal, config)))
                .ToList()
                .AsReadOnly();
            _providers = providers.Select(config => new SubView(config.Name, config.Image, () => regionManager.AddToRegion(RegionNames.Modal, config)))
                .ToList()
                .AsReadOnly();
        }


        public IEnumerable<SubView> ConnectivityConfigurators
        {
            get { return _connectivityConfigurators; }
        }

        public IEnumerable<SubView> Providers
        {
            get { return _providers; }
        }
    }
}
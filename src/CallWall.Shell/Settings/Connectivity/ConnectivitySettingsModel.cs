using System.Collections.Generic;
using System.Linq;

namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsModel : IConnectivitySettingsModel
    {
        private readonly IEnumerable<IConnectionConfiguration> _connectionConfigurations;
        private readonly IPersonalizationSettings _settings;

        public ConnectivitySettingsModel(IEnumerable<IConnectionConfiguration> connectionConfigurations, IPersonalizationSettings settings)
        {
            _connectionConfigurations = connectionConfigurations;
            _settings = settings;
        }

        #region Implementation of IConnectivitySettingsModel

        public bool RequiresSetup
        {
            //get { return _settings.GetAsBool(LocalStoreKeys.ConnectivityRequireSetup, true); }
            get { return ConnectionConfigurations.All(cc => !cc.IsEnabled); }
        }

        public IEnumerable<IConnectionConfiguration> ConnectionConfigurations
        {
            get { return _connectionConfigurations; }
        }

        #endregion
    }
}
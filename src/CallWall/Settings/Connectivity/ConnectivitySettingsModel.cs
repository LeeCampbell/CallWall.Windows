using System.Collections.Generic;

namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsModel : IConnectivitySettingsModel
    {
        private const string RequiresSetupKey = @"CallWall.Settings.Connectivity.ConnectivitySettingsModel.RequiresSetup";

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
            get { return _settings.GetAsBool(RequiresSetupKey, true); }
        }

        public IEnumerable<IConnectionConfiguration> ConnectionConfigurations
        {
            get { return _connectionConfigurations; }
        }

        #endregion
    }
}
using System.Collections.Generic;

namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsModel : IConnectivitySettingsModel
    {
        private readonly IEnumerable<IConnectionConfiguration> _connectionConfigurations;

        public ConnectivitySettingsModel(IEnumerable<IConnectionConfiguration> connectionConfigurations)
        {
            _connectionConfigurations = connectionConfigurations;
        }

        #region Implementation of IConnectivitySettingsModel

        public bool RequiresSetup
        {
            get { return true; }
        }

        public IEnumerable<IConnectionConfiguration> ConnectionConfigurations
        {
            get { return _connectionConfigurations; }
        }

        #endregion
    }
}
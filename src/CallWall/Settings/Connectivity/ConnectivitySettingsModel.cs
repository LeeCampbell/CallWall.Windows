using System.Collections.Generic;

namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsModel : IConnectivitySettingsModel
    {
        private readonly IEnumerable<IConnectivityConfigurator> _connectivityConfigurators;

        public ConnectivitySettingsModel(IEnumerable<IConnectivityConfigurator> connectivityConfigurators)
        {
            _connectivityConfigurators = connectivityConfigurators;
        }

        #region Implementation of IConnectivitySettingsModel

        public bool RequiresSetup
        {
            get { return true; }
        }

        public IEnumerable<IConnectivityConfigurator> ConnectivityConfigurators
        {
            get { return _connectivityConfigurators; }
        }

        #endregion
    }
}
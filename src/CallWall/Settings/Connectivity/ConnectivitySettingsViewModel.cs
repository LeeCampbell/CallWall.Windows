namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsViewModel : IConnectivitySettingsViewModel
    {
        private readonly IConnectivitySettingsModel _connectivitySettingsModel;

        public ConnectivitySettingsViewModel(IConnectivitySettingsModel connectivitySettingsModel)
        {
            _connectivitySettingsModel = connectivitySettingsModel;
        }

        public bool RequiresSetup
        {
            get { return _connectivitySettingsModel.RequiresSetup; }
        }
    }
}
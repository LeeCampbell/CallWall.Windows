namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsModel : IConnectivitySettingsModel
    {
        #region Implementation of IConnectivitySettingsModel

        public bool RequiresSetup
        {
            get { return true; }
        }

        #endregion
    }
}
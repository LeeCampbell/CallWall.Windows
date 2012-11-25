namespace CallWall.Settings.Providers
{
    public interface IProviderSettingsModel
    {
        bool RequiresSetup { get; }
    }

    public sealed class ProviderSettingsModel : IProviderSettingsModel
    {
        #region Implementation of IProviderSettingsModel

        public bool RequiresSetup
        {
            get { return true; }
        }

        #endregion
    }
}
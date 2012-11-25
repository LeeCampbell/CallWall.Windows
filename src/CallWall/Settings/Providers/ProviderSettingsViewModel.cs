namespace CallWall.Settings.Providers
{
    public sealed class ProviderSettingsViewModel : IProviderSettingsViewModel
    {
        private readonly IProviderSettingsModel _providerSettingsModel;

        public ProviderSettingsViewModel(IProviderSettingsModel providerSettingsModel)
        {
            _providerSettingsModel = providerSettingsModel;
        }

        public bool RequiresSetup
        {
            get { return _providerSettingsModel.RequiresSetup; }
        }
    }
}
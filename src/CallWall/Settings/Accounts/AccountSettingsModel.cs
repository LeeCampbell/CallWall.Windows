using System.Collections.Generic;

namespace CallWall.Settings.Accounts
{
    public sealed class AccountSettingsModel : IAccountSettingsModel
    {
        private readonly IEnumerable<IAccountConfiguration> _accountConfigurations;
        private readonly IPersonalizationSettings _settings;

        public AccountSettingsModel(IEnumerable<IAccountConfiguration> accountConfigurations, IPersonalizationSettings settings)
        {
            _accountConfigurations = accountConfigurations;
            _settings = settings;
        }

        public bool RequiresSetup
        {
            get { return _settings.GetAsBool(LocalStoreKeys.AccountsRequireSetup, true); }
        }

        public IEnumerable<IAccountConfiguration> AccountConfigurations
        {
            get { return _accountConfigurations; }
        }
    }
}
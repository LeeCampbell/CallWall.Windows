using System.Collections.Generic;
using System.Linq;

namespace CallWall.Windows.Shell.Settings.Accounts
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
            //get { return _settings.GetAsBool(LocalStoreKeys.AccountsRequireSetup, true); }
            get { return AccountConfigurations.All(ac=>!ac.IsEnabled); }
        }

        public IEnumerable<IAccountConfiguration> AccountConfigurations
        {
            get { return _accountConfigurations; }
        }
    }
}
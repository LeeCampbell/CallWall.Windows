using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Accounts
{
    public sealed class AccountSettingsViewModel : IAccountSettingsViewModel
    {
        private readonly IAccountSettingsModel _accountSettingsModel;
        private readonly DelegateCommand _closeCommand;

        public AccountSettingsViewModel(IAccountSettingsModel accountSettingsModel)
        {
            _accountSettingsModel = accountSettingsModel;
            _closeCommand = new DelegateCommand(OnClosed);
        }

        public bool RequiresSetup
        {
            get { return _accountSettingsModel.RequiresSetup; }
        }

        public IEnumerable<IAccountConfiguration> AccountConfigurations
        {
            get { return _accountSettingsModel.AccountConfigurations; }
        }

        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public event EventHandler Closed;

        private void OnClosed()
        {
            var handler = Closed;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
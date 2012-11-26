using System;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Providers
{
    public sealed class ProviderSettingsViewModel : IProviderSettingsViewModel
    {
        private readonly IProviderSettingsModel _providerSettingsModel;
        private readonly DelegateCommand _closeCommand;

        public ProviderSettingsViewModel(IProviderSettingsModel providerSettingsModel)
        {
            _providerSettingsModel = providerSettingsModel;
            _closeCommand = new DelegateCommand(OnClosed);
        }

        public bool RequiresSetup
        {
            get { return _providerSettingsModel.RequiresSetup; }
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
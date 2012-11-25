using System;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Connectivity
{
    public sealed class ConnectivitySettingsViewModel : IConnectivitySettingsViewModel
    {
        private readonly IConnectivitySettingsModel _connectivitySettingsModel;
        private readonly DelegateCommand _closeCommand;

        public ConnectivitySettingsViewModel(IConnectivitySettingsModel connectivitySettingsModel)
        {
            _connectivitySettingsModel = connectivitySettingsModel;
            _closeCommand = new DelegateCommand(OnClosed);
        }

        public bool RequiresSetup
        {
            get { return _connectivitySettingsModel.RequiresSetup; }
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
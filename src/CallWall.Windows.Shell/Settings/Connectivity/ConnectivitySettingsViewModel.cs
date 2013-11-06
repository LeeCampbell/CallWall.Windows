using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Windows.Shell.Settings.Connectivity
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

        public IEnumerable<IConnectionConfiguration> ConnectivityConfigurators
        {
            get { return _connectivitySettingsModel.ConnectionConfigurations; }
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
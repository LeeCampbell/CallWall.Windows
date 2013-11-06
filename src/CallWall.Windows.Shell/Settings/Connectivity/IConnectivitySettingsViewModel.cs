using System;

namespace CallWall.Windows.Shell.Settings.Connectivity
{
    public interface IConnectivitySettingsViewModel
    {
        bool RequiresSetup { get; }
        event EventHandler Closed;
    }
}
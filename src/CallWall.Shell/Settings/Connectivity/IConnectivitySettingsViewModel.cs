using System;

namespace CallWall.Settings.Connectivity
{
    public interface IConnectivitySettingsViewModel
    {
        bool RequiresSetup { get; }
        event EventHandler Closed;
    }
}
using System;

namespace CallWall.Settings.Providers
{
    public interface IProviderSettingsViewModel
    {
        bool RequiresSetup { get; }
        event EventHandler Closed;
    }
}
using System;

namespace CallWall.Settings.Accounts
{
    public interface IAccountSettingsViewModel
    {
        bool RequiresSetup { get; }
        event EventHandler Closed;
    }
}
using System;

namespace CallWall.Windows.Shell.Settings.Accounts
{
    public interface IAccountSettingsViewModel
    {
        bool RequiresSetup { get; }
        event EventHandler Closed;
    }
}
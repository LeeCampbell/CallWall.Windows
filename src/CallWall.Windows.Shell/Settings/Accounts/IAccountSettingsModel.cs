using System.Collections.Generic;

namespace CallWall.Windows.Shell.Settings.Accounts
{
    public interface IAccountSettingsModel
    {
        bool RequiresSetup { get; }
        IEnumerable<IAccountConfiguration> AccountConfigurations { get; }
    }
}
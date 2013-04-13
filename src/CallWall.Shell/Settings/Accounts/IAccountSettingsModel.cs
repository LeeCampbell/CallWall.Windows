using System.Collections.Generic;

namespace CallWall.Settings.Accounts
{
    public interface IAccountSettingsModel
    {
        bool RequiresSetup { get; }
        IEnumerable<IAccountConfiguration> AccountConfigurations { get; }
    }
}
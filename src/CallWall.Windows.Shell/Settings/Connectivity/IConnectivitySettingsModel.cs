using System.Collections.Generic;

namespace CallWall.Windows.Shell.Settings.Connectivity
{
    public interface IConnectivitySettingsModel
    {
        bool RequiresSetup { get; }
        IEnumerable<IConnectionConfiguration> ConnectionConfigurations { get; }
    }
}
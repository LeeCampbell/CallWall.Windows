using System.Collections.Generic;

namespace CallWall.Settings.Connectivity
{
    public interface IConnectivitySettingsModel
    {
        bool RequiresSetup { get; }
        IEnumerable<IConnectionConfiguration> ConnectionConfigurations { get; }
    }
}
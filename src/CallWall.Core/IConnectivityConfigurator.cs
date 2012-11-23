using System;
using System.ComponentModel;

namespace CallWall
{
    public interface IConnectivityConfigurator : INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }//?
        string Name { get; }
        Uri Image { get; }
    }
}

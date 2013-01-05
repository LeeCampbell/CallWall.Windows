using System.ComponentModel;

namespace CallWall.Settings.Connectivity.Bluetooth
{
    public interface IBluetoothSetupViewModel : INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }
    }
}
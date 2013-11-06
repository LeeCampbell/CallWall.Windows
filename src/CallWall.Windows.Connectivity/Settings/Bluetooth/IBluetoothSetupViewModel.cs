using System.ComponentModel;

namespace CallWall.Windows.Connectivity.Settings.Bluetooth
{
    public interface IBluetoothSetupViewModel : INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }
    }
}
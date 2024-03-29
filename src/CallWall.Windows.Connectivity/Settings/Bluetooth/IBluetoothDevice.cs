using System.ComponentModel;
using CallWall.Windows.Shell;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Windows.Connectivity.Settings.Bluetooth
{
    public interface IBluetoothDevice : INotifyPropertyChanged
    {
        string Name { get; }
        BluetoothDeviceType DeviceType { get; }
        ViewModelStatus Status { get; }
        DelegateCommand PairDeviceCommand { get; }
        DelegateCommand RemoveDeviceCommand { get; }
    }
}
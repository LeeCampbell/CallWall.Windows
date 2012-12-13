using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Bluetooth
{
    public interface IBluetoothDevice : INotifyPropertyChanged
    {
        string Name { get; }
        BluetoothDeviceType DeviceType { get; }
        ViewModelStatus Status { get; set; }
        DelegateCommand PairDeviceCommand { get; }
        DelegateCommand RemoveDeviceCommand { get; }
    }
}
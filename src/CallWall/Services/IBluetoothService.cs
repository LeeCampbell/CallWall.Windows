using System;
using System.ComponentModel;
using CallWall.Settings.Connectivity.Bluetooth;

namespace CallWall.Services
{
    public interface IBluetoothService : INotifyPropertyChanged
    {
        bool IsSupported { get; }
        bool IsEnabled { get; set; }

        IObservable<IBluetoothDevice> ScanForDevices();

        IObservable<bool> PairDevice(IBluetoothDeviceInfo device);
        IObservable<bool> RemoveDevice(IBluetoothDeviceInfo device);
    }
}

using System;
using CallWall.Settings.Bluetooth;

namespace CallWall.Services
{
    public interface IBluetoothService
    {
        IObservable<IBluetoothDevice> SearchForDevices();

        IObservable<bool> PairDevice(IBluetoothDeviceInfo device);
        IObservable<bool> RemoveDevice(IBluetoothDeviceInfo device);
    }
}

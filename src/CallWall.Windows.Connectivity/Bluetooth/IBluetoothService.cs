using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using CallWall.Windows.Connectivity.Settings.Bluetooth;

namespace CallWall.Windows.Connectivity.Bluetooth
{
    public interface IBluetoothService
    {
        bool IsSupported { get; }

        IObservable<IBluetoothDevice> ScanForDevices();

        IObservable<bool> PairDevice(IBluetoothDeviceInfo device);
        IObservable<bool> RemoveDevice(IBluetoothDeviceInfo device);

        IObservable<IList<string>> IdentitiesActivated(IScheduler scheduler);
    }
}

using System;

namespace CallWall.Settings.Bluetooth
{
    public interface IBluetoothSetup
    {
        IObservable<BluetoothDevice> SearchForDevices();
    }
}
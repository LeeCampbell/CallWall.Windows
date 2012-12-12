﻿using System;
using CallWall.Settings.Bluetooth;
using InTheHand.Net.Sockets;

namespace CallWall.Services
{
    public interface IBluetoothService
    {
        IObservable<BluetoothDevice> SearchForDevices();

        IObservable<bool> PairDevice(BluetoothDeviceInfo device);
        IObservable<bool> RemoveDevice(BluetoothDeviceInfo device);
    }
}
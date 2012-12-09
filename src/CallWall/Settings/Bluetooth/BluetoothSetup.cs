using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothSetup : IBluetoothSetup
    {
        //private static readonly Guid CallMeServiceId  = new Guid("5DFEE4FE-A594-4BFB-B21A-6D7184330669"); //My generated random Id.
        //private static readonly Guid CallMeServiceId = new Guid("00001105-0000-1000-8000-00805F9B34FB");  //Std generic id
        private static readonly Guid CallMeServiceId = new Guid("fa87c0d0-afac-11de-8a39-0800200c9a66"); //Bluetooth chat id

        public BluetoothSetup()
        {
        }

        #region Implementation of IBluetoothSetup

        public IObservable<BluetoothDevice> SearchForDevices()
        {
            return Observable.Create<BluetoothDevice>(o =>
            {
                using (var btClient = new BluetoothClient())
                {
                    var devices = btClient.DiscoverDevices();
                    foreach (var bluetoothDeviceInfo in devices)
                    {
                        var deviceType = BluetoothDeviceType.Create(bluetoothDeviceInfo.ClassOfDevice.Device);
                        var signalStrength = bluetoothDeviceInfo.Rssi;  //Implies connected?
                        var isRemembered = bluetoothDeviceInfo.Remembered;  //If signalStrength is 0, then assume it is remembered?
                        Console.WriteLine("{0} strength = {1}", bluetoothDeviceInfo.DeviceName, signalStrength);
                        var btd = new BluetoothDevice(bluetoothDeviceInfo.DeviceName, deviceType, bluetoothDeviceInfo.DeviceAddress);
                        o.OnNext(btd);
                    }
                    o.OnCompleted();
                    //TODO: implement cancelation properly. -LC
                    return Disposable.Empty;
                }
            });
        }

       

        #endregion
    }
}
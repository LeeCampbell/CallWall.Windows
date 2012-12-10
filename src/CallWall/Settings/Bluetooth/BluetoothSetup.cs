using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using InTheHand.Net.Sockets;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothSetup : IBluetoothSetup
    {
        //private static readonly Guid CallMeServiceId  = new Guid("5DFEE4FE-A594-4BFB-B21A-6D7184330669"); //My generated random Id.
        //private static readonly Guid CallMeServiceId = new Guid("00001105-0000-1000-8000-00805F9B34FB");  //Std generic id
        private static readonly Guid CallMeServiceId = new Guid("fa87c0d0-afac-11de-8a39-0800200c9a66"); //Bluetooth chat id

        private readonly IBluetoothDeviceFactory _bluetoothDeviceFactory;

        public BluetoothSetup(IBluetoothDeviceFactory bluetoothDeviceFactory)
        {
            _bluetoothDeviceFactory = bluetoothDeviceFactory;
        }

        public IObservable<BluetoothDevice> SearchForDevices()
        {
            return Observable.Create<BluetoothDevice>(o =>
            {
                using (var btClient = new BluetoothClient())
                {
                    var devices = btClient.DiscoverDevices();
                    foreach (var bluetoothDeviceInfo in devices)
                    {
                        var btd = _bluetoothDeviceFactory.Create(bluetoothDeviceInfo);
                        o.OnNext(btd);
                    }
                    o.OnCompleted();
                    //TODO: implement cancelation properly. -LC
                    return Disposable.Empty;
                }
            });
        }
    }
}
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Settings.Bluetooth;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace CallWall.Services
{
    public sealed class BluetoothService : IBluetoothService
    {
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;
        private const string Pin = "127025";//TODO: Validate what this is used for. May need to rand generate it for security purposes. -LC

        //private static readonly Guid CallMeServiceId  = new Guid("5DFEE4FE-A594-4BFB-B21A-6D7184330669"); //My generated random Id.
        //private static readonly Guid CallMeServiceId = new Guid("00001105-0000-1000-8000-00805F9B34FB");  //Std generic id
        private static readonly Guid CallMeServiceId = new Guid("fa87c0d0-afac-11de-8a39-0800200c9a66"); //Bluetooth chat id

        public BluetoothService(ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger();
        }

        public IObservable<IBluetoothDevice> SearchForDevices()
        {
            return Observable.Create<BluetoothDevice>(
                o =>
                {
                    using (var btClient = new BluetoothClient())
                    {
                        var devices = btClient.DiscoverDevices();
                        _logger.Debug("Found {0} Bluetooth devices", devices.Length);
                        foreach (var bluetoothDeviceInfo in devices)
                        {
                            var btd = Create(bluetoothDeviceInfo);
                            o.OnNext(btd);
                        }
                        o.OnCompleted();
                        //TODO: implement cancelation properly. -LC
                        return Disposable.Empty;
                    }
                })
                .Log(_logger, "SearchForDevices()");
        }

        public IObservable<bool> PairDevice(BluetoothDeviceInfo device)
        {
            return Observable.Create<bool>(observer =>
                {
                    _logger.Info("Request to pair Bluetooth device {0} ({1})", device.DeviceName, device.ClassOfDevice.Device);
                    try
                    {
                        var successful = BluetoothSecurity.PairRequest(device.DeviceAddress, Pin);
                        _logger.Info("Request to pair Bluetooth device '{0}' ({1}) was {2}successful",
                            device.DeviceName,
                            device.ClassOfDevice.Device,
                            successful ? string.Empty : "un");

                        _logger.Debug("Refreshing Bluetooth device '{0}' ({1}) state", device.DeviceName, device.ClassOfDevice.Device);

                        device.Refresh();

                        _logger.Debug("Bluetooth device '{0}' ({1}) is {2} connected",
                                device.DeviceName,
                                device.ClassOfDevice.Device,
                                device.Connected ? "now" : "not");

                        observer.OnNext(successful);
                        observer.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn(ex, "Request to pair Bluetooth device '{0}' ({1}) caused an error",
                           device.DeviceName,
                           device.ClassOfDevice.Device);
                        observer.OnNext(false);
                        observer.OnCompleted();
                    }
                    //TODO: Correctly implement cancelation - LC
                    return Disposable.Empty;
                });
        }

        public IObservable<bool> RemoveDevice(BluetoothDeviceInfo device)
        {
            return Observable.Create<bool>(observer =>
            {
                _logger.Info("Request to remove Bluetooth device {0} ({1})", device.DeviceName, device.ClassOfDevice.Device);
                try
                {
                    var successful = BluetoothSecurity.RemoveDevice(device.DeviceAddress);
                    _logger.Info("Request to remove Bluetooth device '{0}' ({1}) was {2}successful",
                            device.DeviceName,
                            device.ClassOfDevice.Device,
                            successful ? string.Empty : "un");
                    _logger.Debug("Refreshing Bluetooth device '{0}' ({1}) state", device.DeviceName, device.ClassOfDevice.Device);

                    device.Refresh();

                    _logger.Debug("Bluetooth device '{0}' ({1}) is {2} connected",
                            device.DeviceName,
                            device.ClassOfDevice.Device,
                            device.Connected ? "now" : "not");
                    observer.OnNext(successful);
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "Request to remove Bluetooth device '{0}' ({1}) caused an error",
                            device.DeviceName,
                            device.ClassOfDevice.Device);
                    observer.OnNext(false);
                    observer.OnCompleted();
                }

                //TODO: Correctly implement cancelation - LC
                return Disposable.Empty;
            });
        }

        private BluetoothDevice Create(BluetoothDeviceInfo btDeviceInfo)
        {
            return new BluetoothDevice(btDeviceInfo, this, _schedulerProvider);
        }
    }
}
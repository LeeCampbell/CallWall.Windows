using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CallWall.Windows.Connectivity.Settings.Bluetooth;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallWall.Windows.Connectivity.Bluetooth
{
    public sealed class BluetoothService : IBluetoothService
    {
        private const string Pin = "127025";//TODO: Validate what this is used for. May need to rand generate it for security purposes. -LC
        //private static readonly Guid _callMeServiceId = new Guid("5DFEE4FE-A594-4BFB-B21A-6D7184330669"); //My generated random Id.
        private static readonly Guid _commonSerialBoardServiceId = new Guid("00001101-0000-1000-8000-00805F9B34FB"); //My generated random Id.

        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;

        public BluetoothService(ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public bool IsSupported
        {
            get { return BluetoothRadio.IsSupported; }
        }

        public IObservable<IBluetoothDevice> ScanForDevices()
        {
            return Observable.Create<IBluetoothDevice>(
                o =>
                {
                    if (!IsSupported)
                    {
                        o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                        return Disposable.Empty;
                    }

                    return Observable.Using(() => new BluetoothClient(), DiscoverDevices)
                                     .SelectMany(devices => devices)
                                     .Select(Create)
                                     .Subscribe(o);
                })
                .Log(_logger, "ScanForDevices()");
        }

        public IObservable<bool> PairDevice(IBluetoothDeviceInfo device)
        {
            return ActionDevice("pair", device, btAddress => BluetoothSecurity.PairRequest(btAddress, Pin));
        }

        public IObservable<bool> RemoveDevice(IBluetoothDeviceInfo device)
        {
            return ActionDevice("remove", device, BluetoothSecurity.RemoveDevice);
        }

        public IObservable<IList<string>> IdentitiesActivated(IScheduler scheduler)
        {
            return Observable.Create<IList<string>>(
                o =>
                {
                    if (!IsSupported)
                    {
                        return Observable.Empty<IList<string>>().Subscribe(o);
                        o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                        return Disposable.Empty;
                    }

                    var resources = new CompositeDisposable();
                    try
                    {
                        var listener = StartBluetoothListener();
                        resources.Add(Disposable.Create(listener.Stop));

                        var subscription = Task.Factory.FromAsync<BluetoothClient>(listener.BeginAcceptBluetoothClient,
                                                                                   listener.EndAcceptBluetoothClient,
                                                                                   null, TaskCreationOptions.None)
                                               .ToObservable()
                                               .Log(_logger, "AcceptBluetoothClient")
                                               .SelectMany(btClient => this.GetIdentities(btClient, scheduler))
                                               .Subscribe(o);

                        resources = new CompositeDisposable(subscription, resources);
                    }
                    catch (Exception ex)
                    {
                        o.OnError(ex);
                    }
                    return resources;
                })
                .Log(_logger, "IdentitiesActivated")
                .SubscribeOn(scheduler);
        }

        private IObservable<IList<string>> GetIdentities(BluetoothClient bluetoothClient, IScheduler scheduler)
        {
            return Observable.Using(() => bluetoothClient,
                                    btClient =>
                                        {
                                            var encoder = new ASCIIEncoding();
                                            _logger.Debug("bluetoothClient.GetStream()...;");
                                            var ns = btClient.GetStream();
                                            _logger.Debug("bluetoothClient.GetStream().;");



                                            return ns.ToObservable(1, scheduler)
                                                     .Aggregate(new List<byte>(),
                                                                (acc, cur) =>
                                                                    {
                                                                        acc.Add(cur);
                                                                        return acc;
                                                                    })
                                                     .Select(bytes => encoder.GetString(bytes.ToArray()))
                                                     .Select(data => data.Split('\n'));
                                        });
        }

        private static IObservable<IList<BluetoothDeviceInfo>> DiscoverDevices(BluetoothClient bluetoothClient)
        {
            return Task.Factory
                       .FromAsync<BluetoothDeviceInfo[]>(
                            (callback, state) =>
                            bluetoothClient.BeginDiscoverDevices(byte.MaxValue, true, true, true, false, callback, state),
                            bluetoothClient.EndDiscoverDevices,
                            null)
                        .ToObservable();
        }

        private IObservable<bool> ActionDevice(string actionName, IBluetoothDeviceInfo device, Func<BluetoothAddress, bool> action)
        {
            return Observable.Create<bool>(o =>
            {
                _logger.Info("Request to {0} Bluetooth device {1} ({2})", actionName, device.DeviceName, device.DeviceType.Name);
                if (!IsSupported)
                {
                    o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                    return Disposable.Empty;
                }

                try
                {
                    var btAddress = new BluetoothAddress(device.DeviceAddress);
                    var successful = action(btAddress);
                    LogDeviceAction(actionName, device, successful);

                    RefreshDevice(device);

                    o.OnNext(successful);
                    o.OnCompleted();
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "Request to {0} Bluetooth device '{1}' ({2}) caused an error",
                        actionName,
                        device.DeviceName,
                        device.DeviceType.Name);
                    o.OnNext(false);
                    o.OnCompleted();
                }
                //Cancellation not supported by BluetoothSecurity api.
                return Disposable.Empty;
            });
        }

        private BluetoothListener StartBluetoothListener()
        {
            _logger.Debug("Creating BluetoothListener({0})", _commonSerialBoardServiceId);
            var listener = new BluetoothListener(_commonSerialBoardServiceId);
            _logger.Debug("Starting Bluetooth listener...");
            listener.Start();
            _logger.Debug("Bluetooth listener started.");
            return listener;
        }

        private BluetoothDevice Create(BluetoothDeviceInfo btDeviceInfo)
        {
            var btInfo = new WrappedBluetoothInfo(btDeviceInfo);
            return new BluetoothDevice(btInfo, this, _schedulerProvider);
        }

        private void LogDeviceAction(string action, IBluetoothDeviceInfo device, bool successful)
        {
            _logger.Info("Request to {0} Bluetooth device '{1}' ({2}) was {3}successful",
                         action,
                         device.DeviceName,
                         device.DeviceType.Name,
                         successful ? string.Empty : "un");
        }

        private void RefreshDevice(IBluetoothDeviceInfo device)
        {
            _logger.Debug("Refreshing Bluetooth device '{0}' ({1}) state", device.DeviceName, device.DeviceType);
            device.Refresh();
            _logger.Debug("Bluetooth device '{0}' ({1}) is {2} connected",
                          device.DeviceName,
                          device.DeviceType,
                          device.IsConnected ? "now" : "not");
        }


    }
}
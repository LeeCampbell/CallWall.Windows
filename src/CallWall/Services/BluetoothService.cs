using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using CallWall.Settings.Connectivity.Bluetooth;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using JetBrains.Annotations;

namespace CallWall.Services
{
    public sealed class BluetoothService : IBluetoothService
    {
        private const string Pin = "127025";//TODO: Validate what this is used for. May need to rand generate it for security purposes. -LC
        private static readonly Guid _callMeServiceId = new Guid("5DFEE4FE-A594-4BFB-B21A-6D7184330669"); //My generated random Id.

        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;
        private bool _isEnabled;

        public BluetoothService(ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger();
            
            //TODO: Load IsEnabled from personalization settings -LC
            IsEnabled = true;

            //HACK:
            //IdentitiesActivated(_schedulerProvider.LongRunning)
            //    .Retry().Repeat()
            //    .Log(_logger, "IdentitiesActivated")
            //    .Subscribe(stuff => _logger.Debug(string.Join(",", stuff)));
        }

        public bool IsSupported
        {
            get { return BluetoothRadio.IsSupported; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value.Equals(_isEnabled)) return;
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
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
                .Log(_logger, "ScanForDevices()");
        }

        public IObservable<bool> PairDevice(IBluetoothDeviceInfo device)
        {
            return Observable.Create<bool>(o =>
                {
                    _logger.Info("Request to pair Bluetooth device {0} ({1})", device.DeviceName, device.DeviceType);
                    if (!IsSupported)
                    {
                        o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                        return Disposable.Empty;
                    }


                    try
                    {
                        var btAddress = new BluetoothAddress(device.DeviceAddress);
                        var successful = BluetoothSecurity.PairRequest(btAddress, Pin);
                        _logger.Info("Request to pair Bluetooth device '{0}' ({1}) was {2}successful",
                            device.DeviceName,
                            device.DeviceType,
                            successful ? string.Empty : "un");

                        _logger.Debug("Refreshing Bluetooth device '{0}' ({1}) state", device.DeviceName, device.DeviceType);

                        device.Refresh();

                        _logger.Debug("Bluetooth device '{0}' ({1}) is {2} connected",
                                device.DeviceName,
                                device.DeviceType,
                                device.IsConnected ? "now" : "not");

                        o.OnNext(successful);
                        o.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn(ex, "Request to pair Bluetooth device '{0}' ({1}) caused an error",
                           device.DeviceName,
                           device.DeviceType);
                        o.OnNext(false);
                        o.OnCompleted();
                    }
                    //TODO: Correctly implement cancelation - LC
                    return Disposable.Empty;
                });
        }

        public IObservable<bool> RemoveDevice(IBluetoothDeviceInfo device)
        {
            return Observable.Create<bool>(o =>
            {
                _logger.Info("Request to remove Bluetooth device {0} ({1})", device.DeviceName, device.DeviceType);
                if (!IsSupported)
                {
                    o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                    return Disposable.Empty;
                }
                try
                {
                    var btAddress = new BluetoothAddress(device.DeviceAddress);

                    var successful = BluetoothSecurity.RemoveDevice(btAddress);
                    _logger.Info("Request to remove Bluetooth device '{0}' ({1}) was {2}successful",
                            device.DeviceName,
                            device.DeviceType,
                            successful ? string.Empty : "un");
                    _logger.Debug("Refreshing Bluetooth device '{0}' ({1}) state", device.DeviceName, device.DeviceType);

                    device.Refresh();

                    _logger.Debug("Bluetooth device '{0}' ({1}) is {2} connected",
                            device.DeviceName,
                            device.DeviceType,
                            device.IsConnected ? "now" : "not");
                    o.OnNext(successful);
                    o.OnCompleted();
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "Request to remove Bluetooth device '{0}' ({1}) caused an error",
                            device.DeviceName,
                            device.DeviceType);
                    o.OnNext(false);
                    o.OnCompleted();
                }

                //TODO: Correctly implement cancelation - LC
                return Disposable.Empty;
            });
        }

        private IObservable<IList<string>> IdentitiesActivated(IScheduler scheduler)
        {
            return Observable.Create<IList<string>>(
                o =>
                {
                    if (!IsSupported)
                    {
                        o.OnError(new InvalidOperationException("Bluetooth not currently supported on this device."));
                        return Disposable.Empty;
                    }

                    var encoder = new ASCIIEncoding();
                    var resources = new CompositeDisposable();

                    try
                    {
                        var listener = StartBluetoothListener();
                        resources.Add(Disposable.Create(listener.Stop));

                        _logger.Debug("_listener.AcceptBluetoothClient();");
                        var bluetoothClient = listener.AcceptBluetoothClient();
                        resources = new CompositeDisposable(bluetoothClient, resources);

                        _logger.Debug("bluetoothClient.GetStream();");
                        var ns = bluetoothClient.GetStream();

                        //TODO: Should this be a recursive call, or should I just continue on the same scheduler path? -LC
                        var subscription = ns.ToObservable(1, scheduler)
                                 .Aggregate(new List<byte>(),
                                            (acc, cur) =>
                                            {
                                                acc.Add(cur);
                                                return acc;
                                            })
                                 .Select(bytes => encoder.GetString(bytes.ToArray()))
                                 .Select(data => data.Split('\n'))
                                 .Subscribe(o);
                        resources = new CompositeDisposable(subscription, resources);
                    }
                    catch (Exception ex)
                    {
                        o.OnError(ex);
                    }
                    return resources;
                }).SubscribeOn(scheduler);
        }

        private BluetoothListener StartBluetoothListener()
        {
            _logger.Debug("Creating BluetoothListener({0})", _callMeServiceId);
            var listener = new BluetoothListener(_callMeServiceId);
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

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
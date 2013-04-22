using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Activators;
using CallWall.Shell.Properties;
using CallWall.Services;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Connectivity.Bluetooth
{
    //This class will need to expose the ability to 
    //  Turn on Bluetooth radio
    //  show available devices
    //  pair with an available Bluetooth device
    //  un-pair with a known device
    //  Test a connection (i.e. validate that CallWall is installed on the paired device)
    public sealed class BluetoothSetupViewModel : IBluetoothSetupViewModel
    {
        #region Field members

        private readonly IBluetoothService _bluetoothService;
        private readonly IBluetoothProfileActivator _bluetoothProfileActivator;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ObservableCollection<IBluetoothDevice> _devices = new ObservableCollection<IBluetoothDevice>();
        private readonly ReadOnlyObservableCollection<IBluetoothDevice> _roDevices;

        private readonly DelegateCommand _scanForDevicesCommand;
        private ViewModelStatus _status = ViewModelStatus.Idle;

        #endregion

        public BluetoothSetupViewModel(IBluetoothService bluetoothService, IBluetoothProfileActivator bluetoothProfileActivator, ISchedulerProvider schedulerProvider)
        {
            _bluetoothService = bluetoothService;
            _bluetoothProfileActivator = bluetoothProfileActivator;
            _schedulerProvider = schedulerProvider;
            _roDevices = new ReadOnlyObservableCollection<IBluetoothDevice>(_devices);
            _scanForDevicesCommand = new DelegateCommand(ScanForDevices, CanScanForDevices);
            _status = ViewModelStatus.Error(Resources.Bluetooth_NoDevices_RequiresScan);
            _bluetoothProfileActivator.PropertyChanges(bs => bs.IsEnabled)
                             .Subscribe(_ =>
                                            {
                                                OnPropertyChanged("IsEnabled");
                                                _scanForDevicesCommand.RaiseCanExecuteChanged();
                                            });               
                                 
        }

        public ReadOnlyObservableCollection<IBluetoothDevice> Devices
        {
            get { return _roDevices; }
        }

        public ViewModelStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                    _scanForDevicesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsSupported
        {
            get { return _bluetoothService.IsSupported; }
        }

        public bool IsEnabled
        {
            get { return _bluetoothProfileActivator.IsEnabled; }
            set { _bluetoothProfileActivator.IsEnabled = value; }
        }

        public DelegateCommand ScanForDevicesCommand { get { return _scanForDevicesCommand; } }

        private bool CanScanForDevices()
        {
            return !Status.IsProcessing && IsSupported && IsEnabled;
        }

        private void ScanForDevices()
        {
            _devices.Clear();
            Status = ViewModelStatus.Processing;
            _bluetoothService.ScanForDevices()
                .SubscribeOn(_schedulerProvider.Concurrent)
                .ObserveOn(_schedulerProvider.Dispatcher)
                .Subscribe(
                    device => _devices.Add(device),
                    ex => { Status = ViewModelStatus.Error(ex.Message); },
                    () =>
                        {
                            Status = _devices.Any() 
                                ? ViewModelStatus.Idle 
                                : ViewModelStatus.Error(Resources.Bluetooth_NoDevicesFound);
                            
                        });
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
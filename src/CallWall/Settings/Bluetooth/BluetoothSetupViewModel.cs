using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using CallWall.Services;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Bluetooth
{
    //This class will need to expose the ability to 
    //  Turn on Bluetooth radio
    //  show available devices
    //  pair with an available Bluetooth device
    //  un-pair with a known device
    //  Test a connection (i.e. validate that CallWall is installed on the paired device)
    public sealed class BluetoothSetupViewModel : INotifyPropertyChanged
    {
        private readonly IBluetoothService _bluetoothService;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ObservableCollection<IBluetoothDevice> _devices = new ObservableCollection<IBluetoothDevice>();
        private readonly ReadOnlyObservableCollection<IBluetoothDevice> _roDevices;

        private readonly DelegateCommand _enableBluetoothCommand;
        private readonly DelegateCommand _searchForDevicesCommandCommand;
        private ViewModelStatus _status = ViewModelStatus.Idle;

        public BluetoothSetupViewModel(IBluetoothService bluetoothService, ISchedulerProvider schedulerProvider)
        {
            _bluetoothService = bluetoothService;
            _schedulerProvider = schedulerProvider;
            _roDevices = new ReadOnlyObservableCollection<IBluetoothDevice>(_devices);
            _enableBluetoothCommand = new DelegateCommand(EnableBluetooth);
            _searchForDevicesCommandCommand = new DelegateCommand(SearchForDevices, () => !Status.IsProcessing);
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
                    _searchForDevicesCommandCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand EnableBluetoothCommand { get { return _enableBluetoothCommand; } }
        public DelegateCommand SearchForDevicesCommand { get { return _searchForDevicesCommandCommand; } }

        private void EnableBluetooth()
        {
            throw new System.NotImplementedException();
        }

        private void SearchForDevices()
        {
            _devices.Clear();
            Status = ViewModelStatus.Processing;
            _bluetoothService.SearchForDevices()
                .SubscribeOn(_schedulerProvider.Concurrent)
                .ObserveOn(_schedulerProvider.Async)
                .Subscribe(
                    device => _devices.Add(device),
                    ex => { Status = ViewModelStatus.Error(ex.Message); },
                    () => { Status = ViewModelStatus.Idle; });
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
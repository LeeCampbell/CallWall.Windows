using System;
using System.ComponentModel;
using System.Reactive.Linq;
using CallWall.Services;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothDevice : IBluetoothDevice
    {
        private readonly IBluetoothDeviceInfo _deviceInfo;
        private readonly IBluetoothService _bluetoothService;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly DelegateCommand _pairDeviceCommand;
        private readonly DelegateCommand _removeDeviceCommand;
        private ViewModelStatus _status = ViewModelStatus.Idle;

        public BluetoothDevice(IBluetoothDeviceInfo deviceInfo, IBluetoothService bluetoothService, ISchedulerProvider schedulerProvider)
        {
            _deviceInfo = deviceInfo;
            _bluetoothService = bluetoothService;
            _schedulerProvider = schedulerProvider;
            _pairDeviceCommand = new DelegateCommand(PairDevice, () => !Status.IsProcessing && !_deviceInfo.IsAuthenticated);
            _removeDeviceCommand = new DelegateCommand(RemoveDevice, () => !Status.IsProcessing && _deviceInfo.IsAuthenticated);
        }

        public string Name
        {
            get { return _deviceInfo.DeviceName; }
        }

        public BluetoothDeviceType DeviceType
        {
            get { return _deviceInfo.DeviceType; }
        }

        public ViewModelStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public DelegateCommand PairDeviceCommand
        {
            get { return _pairDeviceCommand; }
        }

        public DelegateCommand RemoveDeviceCommand
        {
            get { return _removeDeviceCommand; }
        }

        private void PairDevice()
        {
            SetDeviceState(_bluetoothService.PairDevice(_deviceInfo));
        }

        private void RemoveDevice()
        {
            SetDeviceState(_bluetoothService.RemoveDevice(_deviceInfo));
        }

        private void SetDeviceState(IObservable<bool> actionResult)
        {
            Status = ViewModelStatus.Processing;
            RefreshCommands();

            actionResult
                .SubscribeOn(_schedulerProvider.Concurrent)
                .ObserveOn(_schedulerProvider.Async)
                .Subscribe(success =>
                {
                    Status = ViewModelStatus.Idle;
                    RefreshCommands();
                });
        }

        private void RefreshCommands()
        {
            _pairDeviceCommand.RaiseCanExecuteChanged();
            _removeDeviceCommand.RaiseCanExecuteChanged();
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

        public override string ToString()
        {
            return string.Format("BluetoothDevice[Name='{0}', DeviceType='{1}']", Name, DeviceType.Name);
        }
    }
}
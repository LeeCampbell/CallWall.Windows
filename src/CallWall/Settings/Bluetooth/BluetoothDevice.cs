using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothDevice
    {
        private readonly BluetoothDeviceInfo _deviceInfo;
        private readonly ILogger _logger;
        private readonly BluetoothDeviceType _deviceType;
        private readonly DelegateCommand _pairDeviceCommand;


        public BluetoothDevice(BluetoothDeviceInfo deviceInfo, ILoggerFactory loggerFactory)
        {
            _deviceInfo = deviceInfo;
            _logger = loggerFactory.CreateLogger();
            _deviceType = BluetoothDeviceType.Create(deviceInfo.ClassOfDevice.Device);
            _pairDeviceCommand = new DelegateCommand(PairDevice, () => !_deviceInfo.Authenticated);
        }

        public string Name
        {
            get { return _deviceInfo.DeviceName; }
        }

        public BluetoothDeviceType DeviceType
        {
            get { return _deviceType; }
        }

        public DelegateCommand PairDeviceCommand
        {
            get { return _pairDeviceCommand; }
        }

        public void PairDevice()
        {
            _logger.Info("Request Bluetooth pairing for Device {0} ({1})", _deviceInfo.DeviceName, _deviceInfo.ClassOfDevice.Device);
            var wasPaired = BluetoothSecurity.PairRequest(_deviceInfo.DeviceAddress, "127743");

            if (wasPaired)
            {
                _deviceInfo.Refresh();
                _pairDeviceCommand.RaiseCanExecuteChanged();
            }
            _logger.Info("Request for Bluetooth pairing for Device {0} ({1}) were {2}successful and the device is {3} connected",
                _deviceInfo.DeviceName,
                _deviceInfo.ClassOfDevice.Device,
                wasPaired ? string.Empty : "un",
                _deviceInfo.Connected ? "now" : "not");
        }
    }
}
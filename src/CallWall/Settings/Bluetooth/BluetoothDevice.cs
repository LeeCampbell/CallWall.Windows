using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothDevice
    {
        private readonly string _name;
        private readonly BluetoothDeviceType _deviceType;
        private readonly BluetoothAddress _deviceAddress;
        private readonly DelegateCommand _pairDeviceCommand;

        public BluetoothDevice(string name, BluetoothDeviceType deviceType, BluetoothAddress deviceAddress)
        {
            _name = name;
            _deviceType = deviceType;
            _deviceAddress = deviceAddress;
            _pairDeviceCommand = new DelegateCommand(PairDevice);
        }

        public string Name
        {
            get { return _name; }
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
            BluetoothSecurity.PairRequest(_deviceAddress, "1277");
        }
    }
}
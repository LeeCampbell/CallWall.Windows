using CallWall.Windows.Connectivity.Settings.Bluetooth;
using InTheHand.Net.Sockets;

namespace CallWall.Windows.Connectivity.Bluetooth
{
    internal class WrappedBluetoothInfo : IBluetoothDeviceInfo
    {
        private readonly BluetoothDeviceInfo _btDeviceInfo;
        private readonly BluetoothDeviceType _deviceType;

        public WrappedBluetoothInfo(BluetoothDeviceInfo btDeviceInfo)
        {
            _btDeviceInfo = btDeviceInfo;
            _deviceType = BluetoothDeviceType.Create(btDeviceInfo.ClassOfDevice.Device);
        }

        #region Implementation of IBluetoothDeviceInfo

        public bool IsConnected
        {
            get { return _btDeviceInfo.Connected; }
        }

        public bool IsAuthenticated
        {
            get { return _btDeviceInfo.Authenticated; }
        }

        public BluetoothDeviceType DeviceType
        {
            get { return _deviceType; }
        }

        public string DeviceName
        {
            get { return _btDeviceInfo.DeviceName; }
        }

        public byte[] DeviceAddress
        {
            get { return _btDeviceInfo.DeviceAddress.ToByteArray(); }
        }

        public void Refresh()
        {
            _btDeviceInfo.Refresh();
        }

        #endregion
    }
}
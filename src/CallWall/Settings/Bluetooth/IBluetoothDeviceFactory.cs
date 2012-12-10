using InTheHand.Net.Sockets;

namespace CallWall.Settings.Bluetooth
{
    public interface IBluetoothDeviceFactory
    {
        BluetoothDevice Create(BluetoothDeviceInfo btDeviceInfo);
    }

    public sealed class BluetoothDeviceFactory : IBluetoothDeviceFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public BluetoothDeviceFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        #region Implementation of IBluetoothDeviceFactory

        public BluetoothDevice Create(BluetoothDeviceInfo btDeviceInfo)
        {
            return new BluetoothDevice(btDeviceInfo, _loggerFactory);
        }

        #endregion
    }
}
namespace CallWall.Settings.Connectivity.Bluetooth
{
    public interface IBluetoothDeviceInfo
    {
        bool IsConnected { get; }
        bool IsAuthenticated { get; }
        BluetoothDeviceType DeviceType { get; }
        string DeviceName { get; }
        byte[] DeviceAddress { get; }

        void Refresh();
    }
}
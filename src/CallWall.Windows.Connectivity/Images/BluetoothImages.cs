using System;
using System.Windows.Media.Imaging;

namespace CallWall.Windows.Connectivity.Images
{
    internal static class BluetoothImages
    {
        public static readonly Uri BluetoothIconUri;
        private static readonly Lazy<BitmapSource> _bluetoothIconResolver;
        public static BitmapSource BluetoothIcon { get { return _bluetoothIconResolver.Value; } }

        public static readonly Uri AudioVisualIconUri;
        public static readonly Uri ComputerIconUri;
        public static readonly Uri ImagingIconUri;
        public static readonly Uri MiscellaneousIconUri;
        public static readonly Uri NetworkIconUri;
        public static readonly Uri PeripheralIconUri;
        public static readonly Uri PhoneIconUri;

        static BluetoothImages()
        {
            Ensure.PackUriIsRegistered();

            BluetoothIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Bluetooth_72x72.png");

            _bluetoothIconResolver = new Lazy<BitmapSource>(() => new BitmapImage(BluetoothIconUri));

            AudioVisualIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/AudioVisual.png");
            ComputerIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Computer.png");
            ImagingIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Imaging.png");
            MiscellaneousIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Miscellaneous.png");
            NetworkIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Network.png");
            PeripheralIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Peripheral.png");
            PhoneIconUri = new Uri(@"pack://application:,,,/CallWall.Windows.Connectivity;component/Images/Bluetooth/Phone.png");
        }
    }
}

using System;
using System.ComponentModel;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothConnectivityConfigurator : IConnectivityConfigurator
    {
        private readonly IBluetoothSetupView _view;
        private bool _isEnabled;
        private static readonly Uri _image;

        static BluetoothConnectivityConfigurator()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }
            _image = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth_72x72.png");
        }

        public BluetoothConnectivityConfigurator(IBluetoothSetupView view)
        {
            _view = view;
        }

        #region Implementation of IConnectivityConfigurator

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public string Name
        {
            get { return "Bluetooth"; }
        }

        public Uri Image
        {
            get { return _image; }
        }

        public string Description
        {
            //TODO: Move to a resource.
            get { return "Bluetooth is common radio technology that allows devices to transfer information within a radius of up to 10 meters (32 feet). There are no data costs for using Bluetooth and will conveniently only activate CallWall if the paired phone is nearby. Bluetooth will need to be enabled on both the phone and this client PC."; }
        }

        public object View
        {
            get { return _view; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

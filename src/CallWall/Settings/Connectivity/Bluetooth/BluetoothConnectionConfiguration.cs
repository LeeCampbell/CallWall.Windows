using System;
using System.ComponentModel;

namespace CallWall.Settings.Connectivity.Bluetooth
{
    public sealed class BluetoothConnectionConfiguration : IConnectionConfiguration
    {
        private readonly IBluetoothSetupView _view;
        private static readonly Uri _image;

        static BluetoothConnectionConfiguration()
        {
            Ensure.PackUriIsRegistered();
            _image = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth_72x72.png");
        }

        public BluetoothConnectionConfiguration(IBluetoothSetupView view)
        {
            _view = view;
            _view.ViewModel.WhenPropertyChanges(vm => vm.IsEnabled).Subscribe(_ => OnPropertyChanged("IsEnabled"));
        }

        #region Implementation of IConnectionConfiguration

        public bool IsEnabled
        {
            get { return _view.ViewModel.IsEnabled; }
            set { _view.ViewModel.IsEnabled = value; }
        }

        public string Name
        {
            get { return "Bluetooth"; }
        }

        public Uri Image
        {
            get { return _image; }
        }

        //public string Description
        //{
        //    //TODO: Move to a resource.
        //    get { return "Bluetooth is common radio technology that allows devices to transfer information within a radius of up to 10 meters (32 feet). There are no data costs for using Bluetooth and will conveniently only activate CallWall if the paired phone is nearby. Bluetooth will need to be enabled on both the phone and this client PC."; }
        //}

        public object View
        {
            get { return _view; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

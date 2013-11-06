using System;
using System.ComponentModel;
using CallWall.Windows.Connectivity.Images;

namespace CallWall.Windows.Connectivity.Settings.Bluetooth
{
    public sealed class BluetoothConnectionConfiguration : IConnectionConfiguration
    {
        private readonly IBluetoothSetupView _view;

        public BluetoothConnectionConfiguration(IBluetoothSetupView view)
        {
            _view = view;
            _view.ViewModel.PropertyChanges(vm => vm.IsEnabled).Subscribe(_ => OnPropertyChanged("IsEnabled"));
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
            get { return BluetoothImages.BluetoothIconUri; }
        }

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

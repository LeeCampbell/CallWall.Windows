using System;
using System.ComponentModel;

namespace CallWall.Windows.FakeProvider.Connectivity
{
    public sealed class CloudConnectionConfiguration : IConnectionConfiguration
    {
        private bool _isEnabled;
        private static readonly Uri _image;

        static CloudConnectionConfiguration()
        {
            Ensure.PackUriIsRegistered();
            _image = new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Connectivity/Cloud_72x72.png");
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public string Name { get { return "Cloud"; } }

        public Uri Image { get { return _image; } }

        public object View
        {
            get { return null; }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
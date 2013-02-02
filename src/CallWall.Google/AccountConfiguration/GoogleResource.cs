using System;
using System.ComponentModel;
using CallWall.Contract;
using JetBrains.Annotations;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleResource : IResourceScope, INotifyPropertyChanged
    {
        private readonly string _name;
        private readonly Uri _image;
        private readonly Uri _resource;
        private bool _isEnabled;

        public GoogleResource(string name, string image, Uri resource)
        {
            _name = name;
            _image = new Uri(string.Format("pack://application:,,,/CallWall.Google;component/Images/{0}", image));
            _resource = resource;
        }

        public string Name { get { return _name; } }

        public Uri Resource { get { return _resource; } }

        public Uri Image { get { return _image; } }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
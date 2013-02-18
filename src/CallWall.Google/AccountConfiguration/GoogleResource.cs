using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CallWall.Contract;
using JetBrains.Annotations;

namespace CallWall.Google.AccountConfiguration
{
    //TODO: Hmmm; may these shouldn't be static, or at least the IsEnabled may need to be detached.
    public sealed class GoogleResource : IResourceScope, INotifyPropertyChanged
    {
        private readonly string _name;
        private readonly Uri _image;
        private readonly Uri _resource;
        private bool _isEnabled;

        static GoogleResource()
        {
            Ensure.PackUriIsRegistered();
        }

        private GoogleResource(string name, string image, Uri resource)
        {
            _name = name;
            _image = new Uri(string.Format("pack://application:,,,/CallWall.Google;component/Images/{0}", image));
            _resource = resource;
        }

        public static ReadOnlyCollection<GoogleResource> AvailableResourceScopes()
        {
            return new ReadOnlyCollection<GoogleResource>(new[]
                {
                    new GoogleResource("Contacts", "Contacts_48x48.png", new Uri(@"https://www.google.com/m8/feeds/")),
                    new GoogleResource("Email", "Email_48x48.png", new Uri(@"https://mail.google.com/")),
                    new GoogleResource("Calendar", "Calendar_48x48.png", null)
                });
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
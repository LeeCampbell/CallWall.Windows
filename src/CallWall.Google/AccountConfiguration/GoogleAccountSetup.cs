using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetup : IGoogleAccountSetup
    {
        private readonly ReadOnlyObservableCollection<GoogleResource> _roResources;
        private readonly ObservableCollection<GoogleResource> _resouces = new ObservableCollection<GoogleResource>();
        private bool _isAuthorized;
        private bool _isEnabled;

        public GoogleAccountSetup()
        {
            //TODO: Know what we are and are not Authenticated for. 
            //  A change to this state should be reflected in the Model
            //  Save state to disk (can I encrypt this? Will LocalStorage providers protect i for me?)

            _resouces = new ObservableCollection<GoogleResource>();
            _roResources = new ReadOnlyObservableCollection<GoogleResource>(_resouces);

            _resouces.Add(new GoogleResource("Contacts", "Contacts_48x48.png", new Uri(@"https://www.google.com/m8/feeds/")));
            _resouces.Add(new GoogleResource("Email", "Email_48x48.png", null));
            _resouces.Add(new GoogleResource("Calendar", "Calendar_48x48.png", null));
        }

        public ReadOnlyObservableCollection<GoogleResource> Resources { get { return _roResources; } }
        
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

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set
            {
                if (_isAuthorized != value)
                {
                    _isAuthorized = value;
                    OnPropertyChanged("IsAuthorized");
                }
            }
        }

        public void Authorize()
        {
            throw new NotImplementedException();
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
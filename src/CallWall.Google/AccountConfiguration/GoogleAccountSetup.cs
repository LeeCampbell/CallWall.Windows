using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CallWall.Google.Authorization;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetup : IGoogleAccountSetup
    {
        private const string IsEnabledKey = "CallWall.Google.AccountConfiguration.GoogleAccountSetup.IsEnabled";
        private readonly IPersonalizationSettings _settings;
        private readonly IGoogleAuthorization _authorization;
        private bool _isAuthorized;

        public GoogleAccountSetup(IPersonalizationSettings settings, IGoogleAuthorization authorization)
        {
            _settings = settings;
            _authorization = authorization;
            _authorization.Status.Subscribe(s => IsAuthorized = s.IsAuthorized);

            //TODO: Know what we are and are not Authenticated for. 
            //  A change to this state should be reflected in the Model
            //  Save state to disk (can I encrypt this? Will LocalStorage providers protect i for me?)
        }

        public ReadOnlyCollection<GoogleResource> Resources { get { return _authorization.AvailableResourceScopes; } }

        public bool IsEnabled
        {
            get { return _settings.GetAsBool(IsEnabledKey, false); }
            set
            {
                _settings.SetAsBool(IsEnabledKey, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            private set
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
            _authorization.RequestAccessToken().Subscribe();
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
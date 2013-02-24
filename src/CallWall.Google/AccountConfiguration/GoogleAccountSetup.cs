using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CallWall.Google.Authorization;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetup : IGoogleAccountSetup
    {
        private const string IsEnabledKey = "CallWall.Google.AccountConfiguration.GoogleAccountSetup.IsEnabled";
        private readonly IPersonalizationSettings _settings;
        private readonly IGoogleAuthorization _authorization;
        private readonly ObservableCollection<GoogleResource> _selectedResources = new ObservableCollection<GoogleResource>();

        public GoogleAccountSetup(IPersonalizationSettings settings, IGoogleAuthorization authorization)
        {
            _settings = settings;
            _authorization = authorization;
            _authorization.WhenPropertyChanges(a => a.Status).Subscribe(
                _ =>
                {
                    OnPropertyChanged("IsAuthorized");
                    OnPropertyChanged("IsProcessing");
                });

            //TODO: Know what we are and are not Authenticated for. 
            //  A change to this state should be reflected in the Model
            //  Save state to disk (can I encrypt this? Will LocalStorage providers protect i for me?)
        }

        public ReadOnlyCollection<GoogleResource> Resources { get { return _authorization.AvailableResourceScopes; } }
        public ObservableCollection<GoogleResource> SelectedResources { get { return _selectedResources; } }

        public bool IsAuthorized
        {
            get { return _authorization.Status.IsAuthorized; }
        }

        public bool IsProcessing
        {
            get { return _authorization.Status.IsProcessing; }
        }

        public bool IsEnabled
        {
            get { return _settings.GetAsBool(IsEnabledKey, false); }
            set
            {
                _settings.SetAsBool(IsEnabledKey, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        public void Authorize()
        {
            _authorization.Authorize(SelectedResources)
                .Subscribe(i => { }, ex => { });
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
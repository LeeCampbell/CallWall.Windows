using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CallWall.Google.Authorization;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetup : IGoogleAccountSetup
    {
        private readonly IPersonalizationSettings _settings;
        private readonly IGoogleAuthorization _authorization;
        private readonly ObservableCollection<GoogleResource> _selectedResources = new ObservableCollection<GoogleResource>();

        public GoogleAccountSetup(IPersonalizationSettings settings, IGoogleAuthorization authorization)
        {
            _settings = settings;
            _authorization = authorization;
            _authorization.PropertyChanges(a => a.Status)
                          .Subscribe(status =>
                            {
                                OnPropertyChanged("IsAuthorized");
                                OnPropertyChanged("IsProcessing");
                                SetSelectedResources(status);   //TODO: Could supress IsAuth INPC event.
                            });
            SelectedResources.CollectionChanges()
                             .Subscribe(_ => OnPropertyChanged("IsAuthorized"));

            SetSelectedResources(_authorization.Status);
        }

        
        public ReadOnlyCollection<GoogleResource> Resources { get { return _authorization.AvailableResourceScopes; } }
        public ObservableCollection<GoogleResource> SelectedResources { get { return _selectedResources; } }

        public bool IsAuthorized
        {
            get
            {
                return _authorization.Status.IsAuthorized 
                    && _authorization.Status.AuthorizedUris.SetEquals(SelectedResources.Select(r=>r.Resource));
            }
        }

        public bool IsProcessing
        {
            get { return _authorization.Status.IsProcessing; }
        }

        public bool IsEnabled
        {
            get { return _settings.GetAsBool(LocalStoreKeys.GoogleIsEnabled, false); }
            set
            {
                _settings.SetAsBool(LocalStoreKeys.GoogleIsEnabled, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        public void Authorize()
        {
            _authorization.Authorize(SelectedResources)
                .Subscribe(i => { }, ex => { });
        }

        private void SetSelectedResources(Authorization.AuthorizationStatus status)
        {
            //BUG: Reentrancy problem here. Not sure how -LC
            SelectedResources.Clear();
            if (status.IsAuthorized)
            {
                foreach (var authorizedUri in status.AuthorizedUris)
                {
                    var resource = Resources.Single(r => r.Resource == authorizedUri);
                    SelectedResources.Add(resource);
                }
            }
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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Google.Authorization;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetupViewModel : IGoogleAccountSetupViewModel
    {
        private readonly IPersonalizationSettings _settings;
        private readonly IGoogleAuthorization _authorization;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ObservableCollection<GoogleResource> _selectedResources = new ObservableCollection<GoogleResource>();

        private readonly DelegateCommand _authorizeCommand;
        private bool _isProcessingIsAuthChanged;

        public GoogleAccountSetupViewModel(
            IPersonalizationSettings settings, 
            IGoogleAuthorization authorization,
            ISchedulerProvider schedulerProvider)
        {
            _settings = settings;
            _authorization = authorization;
            _schedulerProvider = schedulerProvider;
            _authorization.PropertyChanges(a => a.Status)
                          .ObserveOn(_schedulerProvider.Dispatcher)
                          .Subscribe(status =>
                              {
                                  _isProcessingIsAuthChanged = true;
                                  SetSelectedResources(status);
                                  OnPropertyChanged("IsAuthorized");
                                  OnPropertyChanged("IsProcessing");
                                  _isProcessingIsAuthChanged = false;
                              });
            SelectedResources.CollectionChanges()
                             .Subscribe(_ =>
                                 {
                                     if (!_isProcessingIsAuthChanged)
                                         OnPropertyChanged("IsAuthorized");
                                 });

            SetSelectedResources(_authorization.Status);


            _authorizeCommand = new DelegateCommand(Authorize, () => !IsAuthorized && IsEnabled);
            this.PropertyChanges(m => m.IsAuthorized).Subscribe(_ => _authorizeCommand.RaiseCanExecuteChanged());
            this.PropertyChanges(m => m.IsEnabled).Subscribe(_ => _authorizeCommand.RaiseCanExecuteChanged());
        }

        public bool IsAuthorized
        {
            get
            {
                return _authorization.Status.IsAuthorized
                    && _authorization.Status.AuthorizedUris.SetEquals(SelectedResources.Select(r => r.Resource));
            }
        }

        public bool IsProcessing
        {
            get { return _authorization.Status.IsProcessing; }
        }

        public DelegateCommand AuthorizeCommand { get { return _authorizeCommand; } }

        public ReadOnlyCollection<GoogleResource> Resources { get { return _authorization.AvailableResourceScopes; } }
        public ObservableCollection<GoogleResource> SelectedResources { get { return _selectedResources; } }

        public bool IsEnabled
        {
            get { return _settings.GetAsBool(LocalStoreKeys.GoogleIsEnabled, false); }
            set
            {
                _settings.SetAsBool(LocalStoreKeys.GoogleIsEnabled, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        private void Authorize()
        {
            _authorization.Authorize(SelectedResources)
                .Subscribe(i => { }, ex => { });
        }

        private void SetSelectedResources(Authorization.AuthorizationStatus status)
        {
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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
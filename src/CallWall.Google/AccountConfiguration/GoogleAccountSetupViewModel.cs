using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountSetupViewModel : INotifyPropertyChanged
    {
        private readonly IGoogleAccountSetup _model;
        private readonly DelegateCommand _authorizeCommand;

        public GoogleAccountSetupViewModel(IGoogleAccountSetup model)
        {
            _model = model;

            _authorizeCommand = new DelegateCommand(_model.Authorize, () => !_model.IsAuthorized && _model.IsEnabled);
            _model.PropertyChanges(m => m.IsAuthorized).Subscribe(_ => _authorizeCommand.RaiseCanExecuteChanged());
            _model.PropertyChanges(m => m.IsEnabled).Subscribe(_ =>
                                                                       {
                                                                           OnPropertyChanged("IsEnabled");
                                                                           _authorizeCommand.RaiseCanExecuteChanged();
                                                                       });
        }

        public bool IsEnabled
        {
            get { return _model.IsEnabled; }
            set { _model.IsEnabled = value; }
        }

        public DelegateCommand AuthorizeCommand { get { return _authorizeCommand; } }

        public ReadOnlyCollection<GoogleResource> Resources { get { return _model.Resources; } }
        public ObservableCollection<GoogleResource> SelectedResources { get { return _model.SelectedResources; } }

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
using System;
using System.ComponentModel;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleAccountConfiguration : IAccountConfiguration
    {
        private readonly IGoogleAccountSetupView _view;
        private static readonly Uri _image;

        static GoogleAccountConfiguration()
        {
            Ensure.PackUriIsRegistered();
            _image = new Uri("pack://application:,,,/CallWall.Google;component/Images/Google_64x64.png");
        }

        public GoogleAccountConfiguration(IGoogleAccountSetupView view)
        {
            _view = view;
            _view.ViewModel.WhenPropertyChanges(vm => vm.IsEnabled).Subscribe(_ => OnPropertyChanged("IsEnabled"));
        }
        
        public string Name { get { return "Google"; } }
        public Uri Image { get { return _image; } }
        public object View { get { return _view; } }

        public bool IsEnabled
        {
            get { return _view.ViewModel.IsEnabled; }
            set { _view.ViewModel.IsEnabled = value; }
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

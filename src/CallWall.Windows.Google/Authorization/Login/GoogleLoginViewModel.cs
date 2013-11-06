using System;
using System.ComponentModel;

namespace CallWall.Windows.Google.Authorization.Login
{
    public class GoogleLoginViewModel : INotifyPropertyChanged
    {
        private Uri _authorizationUri;
        private string _authorizationCode;

        public Uri AuthorizationUri
        {
            get { return _authorizationUri; }
            set
            {
                if (_authorizationUri != value)
                {
                    _authorizationUri = value;
                    OnPropertyChanged("AuthorizationUri");
                }
            }
        }

        public string AuthorizationCode
        {
            get { return _authorizationCode; }
            set
            {
                if (_authorizationCode != value)
                {
                    _authorizationCode = value;
                    OnPropertyChanged("AuthorizationCode");
                }
            }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
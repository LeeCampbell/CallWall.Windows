using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Welcome
{
    public sealed class WelcomeViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
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
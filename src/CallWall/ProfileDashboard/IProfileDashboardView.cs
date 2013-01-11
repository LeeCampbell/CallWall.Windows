using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.ProfileDashboard
{
    //TODO: This should start the chain of View/VM/Model. Model should be listening to Activated identities and loading the Dashboard data.
    public interface IProfileDashboardView
    {
        ProfileDashBoardViewModel ViewModel { get; }
    }

    public class ProfileDashBoardViewModel :INotifyPropertyChanged
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

        public IObservable<Unit> Activated
        {
            get { return Observable.Return(Unit.Default); }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
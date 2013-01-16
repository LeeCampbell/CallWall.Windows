using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileDashBoardViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Private fields

        private readonly IProfileDashboard _profileDashboard;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private DelegateCommand _closeCommand;
        private IContactProfile _contact;

        #endregion

        public ProfileDashBoardViewModel(IProfileDashboard profileDashboard, ISchedulerProvider schedulerProvider)
        {
            _profileDashboard = profileDashboard;
            _schedulerProvider = schedulerProvider;
        }

        public IContactProfile Contact
        {
            get { return _contact; }
            private set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
            }
        }


        public void Load(IProfile profile)
        {
            _subscriptions.Add(SubscribeToContact());
            _profileDashboard.Load(profile);
        }

        private IDisposable SubscribeToContact()
        {
            return _profileDashboard.Contact
                                    .ObserveOn(_schedulerProvider.Async)
                                    .Subscribe(c => Contact = c);
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

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _subscriptions.Dispose();
            _profileDashboard.Dispose();
        }

        #endregion
    }
}
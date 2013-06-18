using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.ProfileDashboard.Communication;
using CallWall.ProfileDashboard.Pictures;
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
        private readonly DashboardCollection<Message> _messages;
        private readonly DashboardCollection<Album> _pictureAlbums;
        private DelegateCommand _closeCommand;
        private IContactProfile _contact;
        private ViewModelStatus _contactStatus = ViewModelStatus.Processing;
        private Uri _avatar;

        #endregion

        public ProfileDashBoardViewModel(IProfileDashboard profileDashboard, ISchedulerProvider schedulerProvider)
        {
            _profileDashboard = profileDashboard;
            _schedulerProvider = schedulerProvider;
            _messages = new DashboardCollection<Message>(_profileDashboard.Messages.ObserveOn(_schedulerProvider.Dispatcher));
            _pictureAlbums = new DashboardCollection<Album>(_profileDashboard.PictureAlbums.ObserveOn(_schedulerProvider.Dispatcher));
        }

        public string ActivatedIdentity
        {
            get { return _profileDashboard.ActivatedIdentity; }
        }

        //HACK: Temp hack until I can merge without full updates (which I imagine will force another call to the web to load the avatars
        //  I think I want this to be another rotating image control bound to Contact.Avatars, making this property redundant.
        public Uri Avatar
        {
            get { return _avatar; }
            set
            {
                if (_avatar != value)
                {
                    _avatar = value;
                    OnPropertyChanged("Avatar");
                }
            }
        }

        public IContactProfile Contact
        {
            get { return _contact; }
            private set
            {
                _contact = value;
                if (_contact != null) Avatar = _contact.Avatars.FirstOrDefault();
                OnPropertyChanged("Contact");
            }
        }
        public ViewModelStatus ContactStatus
        {
            get { return _contactStatus; }
            private set
            {
                if (_contactStatus != value)
                {
                    _contactStatus = value;
                    OnPropertyChanged("ContactStatus");
                }
            }
        }

        public DashboardCollection<Message> Messages
        {
            get { return _messages; }
        }

        public DashboardCollection<Album> PictureAlbums
        {
            get { return _pictureAlbums; }
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
            OnPropertyChanged("ActivatedIdentity");
        }

        private IDisposable SubscribeToContact()
        {
            return _profileDashboard.Contact
                                    .ObserveOn(_schedulerProvider.Dispatcher)
                                    .Subscribe(c => Contact = c,
                                    ex=>ContactStatus = ViewModelStatus.Error(ex.Message),
                                    ()=>ContactStatus = ViewModelStatus.Idle);
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
            _messages.Dispose();
            _pictureAlbums.Dispose();
            _profileDashboard.Dispose();
        }

        #endregion
    }
}
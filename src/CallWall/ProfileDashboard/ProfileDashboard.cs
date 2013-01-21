using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.ProfileDashboard.Communication;
using CallWall.ProfileDashboard.Contact;
using CallWall.ProfileDashboard.Pictures;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileDashboard : IProfileDashboard
    {
        private readonly CompositeDisposable _querySubscriptions = new CompositeDisposable();
        private readonly IContactQueryAggregator _contactQueryAggregator;
        private readonly ICommunicationQueryAggregator _communicationQueryAggregator;
        private readonly IPictureQueryAggregator _pictureQueryAggregator;
        private readonly ILogger _logger;

        //HACK: How can I avoid subject here? -LC
        private readonly ISubject<IContactProfile> _contact = new Subject<IContactProfile>();
        private readonly ISubject<Message> _messages = new Subject<Message>();
        private readonly ISubject<Album> _pictureAlbums = new Subject<Album>();

        public ProfileDashboard(ILoggerFactory loggerFactory,
            IContactQueryAggregator contactQueryAggregator,
            ICommunicationQueryAggregator communicationQueryAggregator,
            IPictureQueryAggregator pictureQueryAggregator)
        {
            _contactQueryAggregator = contactQueryAggregator;
            _communicationQueryAggregator = communicationQueryAggregator;
            _pictureQueryAggregator = pictureQueryAggregator;
            _logger = loggerFactory.CreateLogger();
        }

        #region Implementation of IProfileDashboard

        public IObservable<IContactProfile> Contact
        {
            get { return _contact.AsObservable(); }
        }

        public IObservable<Message> Messages
        {
            get { return _messages.AsObservable(); }
        }

        public IObservable<Album> PictureAlbums
        {
            get { return _pictureAlbums.AsObservable(); }
        } 

        public void Load(IProfile profile)
        {
            _querySubscriptions.Add(QueryContacts(profile));
            _querySubscriptions.Add(QueryMessages(profile));
            _querySubscriptions.Add(QueryPictureAlbums(profile));
        }

        
        #endregion

        private IDisposable QueryContacts(IProfile profile)
        {
            //TODO: What to do when a failure occurs?
            return _contactQueryAggregator.Search(profile)
                                           .Subscribe(_contact);
        }

        private IDisposable QueryMessages(IProfile profile)
        {
            //TODO: What to do when a failure occurs?
            return _communicationQueryAggregator.Search(profile)
                                                .Subscribe(_messages);
        }

        private IDisposable QueryPictureAlbums(IProfile profile)
        {
            return _pictureQueryAggregator.Search(profile)
                                          .Subscribe(_pictureAlbums);
        }


        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _querySubscriptions.Dispose();
        }

        #endregion
    }
}
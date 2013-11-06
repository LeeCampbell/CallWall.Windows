using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Calendar;
using CallWall.Windows.Contract.Contact;
using CallWall.Windows.Shell.ProfileDashboard.Calendar;
using CallWall.Windows.Shell.ProfileDashboard.Communication;
using CallWall.Windows.Shell.ProfileDashboard.Contact;
using CallWall.Windows.Shell.ProfileDashboard.Pictures;

namespace CallWall.Windows.Shell.ProfileDashboard
{
    public sealed class ProfileDashboard : IProfileDashboard
    {
        private readonly CompositeDisposable _querySubscriptions = new CompositeDisposable();
        private readonly IContactQueryAggregator _contactQueryAggregator;
        private readonly ICommunicationQueryAggregator _communicationQueryAggregator;
        private readonly IPictureQueryAggregator _pictureQueryAggregator;
        private readonly ICalendarQueryAggregator _calendarQueryAggregator;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;

        //HACK: How can I avoid subject here? -LC
        private readonly ISubject<IContactProfile> _contact = new Subject<IContactProfile>();
        private readonly ISubject<Message> _messages = new Subject<Message>();
        private readonly ISubject<Album> _pictureAlbums = new Subject<Album>();
        private readonly ISubject<ICalendarEvent> _calendarEvents = new Subject<ICalendarEvent>();

        public ProfileDashboard(ILoggerFactory loggerFactory,
            IContactQueryAggregator contactQueryAggregator,
            ICommunicationQueryAggregator communicationQueryAggregator,
            IPictureQueryAggregator pictureQueryAggregator,
            ICalendarQueryAggregator calendarQueryAggregator,
            ISchedulerProvider schedulerProvider)
        {
            _contactQueryAggregator = contactQueryAggregator;
            _communicationQueryAggregator = communicationQueryAggregator;
            _pictureQueryAggregator = pictureQueryAggregator;
            _calendarQueryAggregator = calendarQueryAggregator;
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger(GetType());
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

        public IObservable<ICalendarEvent> CalendarEvents
        {
            get { return _calendarEvents.AsObservable(); }
        } 

        public void Load(IProfile profile)
        {
            ActivatedIdentity = profile.Identifiers.Where(id => id.IdentifierType == "phone")
                                       .Concat(profile.Identifiers)
                                       .Select(id => id.Value)
                                       .FirstOrDefault();
            _querySubscriptions.Add(QueryContacts(profile));
            _querySubscriptions.Add(QueryMessages(profile));
            _querySubscriptions.Add(QueryPictureAlbums(profile));
            _querySubscriptions.Add(QueryCalendars(profile));
        }

        public string ActivatedIdentity { get; private set; }

        #endregion

        private IDisposable QueryContacts(IProfile profile)
        {
            //TODO: What to do when a failure occurs?
            return _contactQueryAggregator.Search(profile)
                                          .SubscribeOn(_schedulerProvider.Concurrent)
                                          .ObserveOn(_schedulerProvider.Dispatcher)
                                          .Subscribe(_contact);
        }

        private IDisposable QueryMessages(IProfile profile)
        {
            //TODO: What to do when a failure occurs?
            return _communicationQueryAggregator.Search(profile)
                                                .Log(_logger, "QueryMessages")
                                                .SubscribeOn(_schedulerProvider.Concurrent)
                                                .ObserveOn(_schedulerProvider.Dispatcher)
                                                .Subscribe(_messages);
        }

        private IDisposable QueryPictureAlbums(IProfile profile)
        {
            return _pictureQueryAggregator.Search(profile)
                                          .SubscribeOn(_schedulerProvider.Concurrent)
                                          .ObserveOn(_schedulerProvider.Dispatcher)
                                          .Subscribe(_pictureAlbums);
        }

        private IDisposable QueryCalendars(IProfile profile)
        {
            return _calendarQueryAggregator.Search(profile)
                                           .SubscribeOn(_schedulerProvider.Concurrent)
                                           .ObserveOn(_schedulerProvider.Dispatcher)
                                           .Subscribe(_calendarEvents);
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
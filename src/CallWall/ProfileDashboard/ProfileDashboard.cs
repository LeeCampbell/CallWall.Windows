using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.ProfileDashboard.Communication;
using CallWall.ProfileDashboard.Contact;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileDashboard : IProfileDashboard
    {
        private readonly CompositeDisposable _querySubscriptions = new CompositeDisposable();
        private readonly IContactQueryAggregator _contactQueryAggregator;
        private readonly ICommunicationQueryAggregator _communicationQueryAggregator;
        private readonly ILogger _logger;

        //HACK: How can I avoid subject here? -LC
        private readonly ISubject<IContactProfile> _contact = new Subject<IContactProfile>();
        private readonly ISubject<MessageViewModel> _messages = new Subject<MessageViewModel>();

        public ProfileDashboard(ILoggerFactory loggerFactory,
            IContactQueryAggregator contactQueryAggregator,
            ICommunicationQueryAggregator communicationQueryAggregator)
        {
            _contactQueryAggregator = contactQueryAggregator;
            _communicationQueryAggregator = communicationQueryAggregator;
            _logger = loggerFactory.CreateLogger();
        }

        #region Implementation of IProfileDashboard

        public IObservable<IContactProfile> Contact
        {
            get { return _contact.AsObservable(); }
        }

        public IObservable<MessageViewModel> Messages
        {
            get { return _messages; }
        }

        public void Load(IProfile profile)
        {
            _querySubscriptions.Add(QueryContacts(profile));
            _querySubscriptions.Add(QueryMessages(profile));
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
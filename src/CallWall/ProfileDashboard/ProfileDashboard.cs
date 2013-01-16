using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Contract;
using CallWall.ProfileDashboard.Contact;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileDashboard : IProfileDashboard
    {
        private readonly CompositeDisposable _querySubscriptions = new CompositeDisposable();
        private readonly IContactQueryAggregator _contactQueryAggregator;
        private readonly ILogger _logger;

        //HACK: How can I avoid subject here? -LC
        private readonly ISubject<IContactProfile> _contact = new Subject<IContactProfile>();

        public ProfileDashboard(ILoggerFactory loggerFactory, IContactQueryAggregator contactQueryAggregator)
        {
            _contactQueryAggregator = contactQueryAggregator;
            _logger = loggerFactory.CreateLogger();
        }

        #region Implementation of IProfileDashboard

        public IObservable<IContactProfile> Contact
        {
            get { return _contact.AsObservable(); }
        }

        public void Load(IProfile profile)
        {
            _querySubscriptions.Add(QueryContacts(profile));
        }

        
        #endregion

        private IDisposable QueryContacts(IProfile profile)
        {
            //TODO: What to do when a failure occurs?
            return _contactQueryAggregator.Search(profile)
                                           .Subscribe(_contact);
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
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;

namespace CallWall.ProfileDashboard.Contact
{
    public sealed class ContactQueryAggregator : IContactQueryAggregator
    {
        private readonly IEnumerable<IContactQueryProvider> _contactQueryProviders;

        public ContactQueryAggregator(IEnumerable<IContactQueryProvider> contactQueryProviders)
        {
            _contactQueryProviders = contactQueryProviders;
        }

        public IObservable<IContactProfile> Search(IProfile profile)
        {
            var queryResults = from provider in _contactQueryProviders.ToObservable()
                               from contact in provider.LoadContact(profile)
                               select contact;

            var aggergateContact = queryResults.Scan(NullContactProfile.Instance,
                                                     (acc, cur) => new AggregatedContact(acc, cur));

            return aggergateContact;
        }
    }
}
using System;
using System.Collections.Generic;
using CallWall.Contract.Contact;
using Microsoft.Practices.Unity.Utility;

namespace CallWall.ProfileDashboard.Contact
{
    //TODO: Resolve duplicates and conflicts.
    public sealed class AggregatedContact : IContactProfile
    {
        public AggregatedContact(IContactProfile seed, IContactProfile addendum)
        {
            Guard.ArgumentNotNull(seed, "seed");
            Guard.ArgumentNotNull(addendum, "addendum");

            Title = seed.Title ?? addendum.Title;
            FullName = seed.FullName ?? addendum.FullName;
            DateOfBirth = seed.DateOfBirth ?? addendum.DateOfBirth;
            Tags = Concat(seed.Tags, addendum.Tags);
            Avatars = Concat(seed.Avatars, addendum.Avatars);
            Organizations = Concat(seed.Organizations, addendum.Organizations);
            Relationships = Concat(seed.Relationships, addendum.Relationships);
            EmailAddresses = Concat(seed.EmailAddresses, addendum.EmailAddresses);
            PhoneNumbers = Concat(seed.PhoneNumbers, addendum.PhoneNumbers);
        }

        public string Title { get; private set; }
        public string FullName { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
        public IEnumerable<Uri> Avatars { get; private set; }
        public IEnumerable<IContactAssociation> Organizations { get; private set; }
        public IEnumerable<IContactAssociation> Relationships { get; private set; }
        public IEnumerable<IContactAssociation> EmailAddresses { get; private set; }
        public IEnumerable<IContactAssociation> PhoneNumbers { get; private set; }

        private static IEnumerable<T> Concat<T>(IEnumerable<T> first,  IEnumerable<T> second)
        {
            var a = first ?? new List<T>();
            var b = second ?? new List<T>();

            var result = new List<T>(a);
            result.AddRange(b);
            return result;
        }
    }
}
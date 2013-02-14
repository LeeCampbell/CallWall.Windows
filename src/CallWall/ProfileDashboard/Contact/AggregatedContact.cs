using System;
using System.Collections.Generic;
using CallWall.Contract.Contact;
using Microsoft.Practices.Unity.Utility;

namespace CallWall.ProfileDashboard.Contact
{
    public sealed class AggregatedContact : IContactProfile
    {
        public AggregatedContact(IContactProfile seed, IContactProfile addendum)
        {
            Guard.ArgumentNotNull(seed, "seed");
            Guard.ArgumentNotNull(addendum, "addendum");

            Title = seed.Title ?? addendum.Title;
            FullName = seed.FullName ?? addendum.FullName;
            DateOfBirth = seed.DateOfBirth ?? addendum.DateOfBirth;
            Tags = Concat(seed.Tags, addendum.Tags, StringComparer.InvariantCultureIgnoreCase);
            Avatars = Concat(seed.Avatars, addendum.Avatars, EqualityComparer<Uri>.Default);
            Organizations = Concat(seed.Organizations, addendum.Organizations, ContactAssociationComparer.Instance);
            Relationships = Concat(seed.Relationships, addendum.Relationships, ContactAssociationComparer.Instance);
            EmailAddresses = Concat(seed.EmailAddresses, addendum.EmailAddresses, ContactAssociationComparer.Instance);
            PhoneNumbers = Concat(seed.PhoneNumbers, addendum.PhoneNumbers, ContactAssociationComparer.Instance);
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

        private static IEnumerable<T> Concat<T>(IEnumerable<T> first,  IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            var a = first ?? new List<T>();
            var b = second ?? new List<T>();

            var result = new HashSet<T>(a, comparer);
            foreach (var element in b)
            {
                result.Add(element);
            }
            return result;
        }

        private sealed class ContactAssociationComparer : IEqualityComparer<IContactAssociation>
        {
            public static readonly ContactAssociationComparer Instance = new ContactAssociationComparer();

            private ContactAssociationComparer()
            {}

            public bool Equals(IContactAssociation x, IContactAssociation y)
            {
                return string.Equals(x.Association, y.Association, StringComparison.InvariantCultureIgnoreCase)
                       && string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(IContactAssociation obj)
            {
                return obj.Association.GetHashCode() ^ obj.Name.GetHashCode();
            }
        }


    }
}
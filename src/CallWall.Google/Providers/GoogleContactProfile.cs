using System;
using System.Collections.Generic;
using CallWall.Contract.Contact;

namespace CallWall.Google.Providers
{
    public sealed class GoogleContactProfile : IContactProfile
    {
        private readonly IEnumerable<Uri> _avatars;
        private readonly IEnumerable<IContactAssociation> _organizations;
        private readonly IEnumerable<IContactAssociation> _relationships;
        private readonly IEnumerable<IContactAssociation> _emailAddresses;
        private readonly IEnumerable<IContactAssociation> _phoneNumbers;

        public GoogleContactProfile(string title, string fullName, DateTime? dateOfBirth,
            IEnumerable<Uri> avatars,
            IEnumerable<ContactAssociation> emailAddresses,
            IEnumerable<ContactAssociation> phoneNumbers,
            IEnumerable<ContactAssociation> organizations,
            IEnumerable<ContactAssociation> relationships)
        {
            _avatars = avatars;
            _emailAddresses = emailAddresses;
            _phoneNumbers = phoneNumbers;
            _organizations = organizations;
            _relationships = relationships;
            Title = title;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
        }

        /// <summary>
        /// How the user commonly references the contact e.g. Dan Rowe
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The formal or full name of the contact e.g. Mr. Daniel Alex Rowe
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// The Date of birth for the contact. If the Year is unknown then it should be set to a value of 1.
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }

        /// <summary>
        /// Link to an image or avatar of the contact
        /// </summary>
        public IEnumerable<Uri> Avatars
        {
            get { return _avatars; }
        }

        public IEnumerable<IContactAssociation> Organizations
        {
            get { return _organizations; }
        }

        public IEnumerable<IContactAssociation> Relationships
        {
            get { return _relationships; }
        }

        public IEnumerable<IContactAssociation> EmailAddresses
        {
            get { return _emailAddresses; }
        }

        public IEnumerable<IContactAssociation> PhoneNumbers
        {
            get { return _phoneNumbers; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CallWall.Windows.Contract.Contact;

namespace CallWall.Windows.Google.Providers.Contacts
{
    public sealed class GoogleContactProfile : IGoogleContactProfile
    {
        private readonly IEnumerable<Uri> _avatars;
        private readonly IEnumerable<IContactAssociation> _organizations;
        private readonly IEnumerable<IContactAssociation> _relationships;
        private readonly IEnumerable<IContactAssociation> _emailAddresses;
        private readonly IEnumerable<IContactAssociation> _phoneNumbers;
        private readonly IEnumerable<Uri> _groupUris;
        private readonly Collection<string> _tags = new Collection<string>();

        public GoogleContactProfile(string title, string fullName, DateTime? dateOfBirth, 
            IEnumerable<Uri> avatars, 
            IEnumerable<ContactAssociation> emailAddresses, 
            IEnumerable<ContactAssociation> phoneNumbers, 
            IEnumerable<ContactAssociation> organizations, 
            IEnumerable<ContactAssociation> relationships,
            IEnumerable<Uri> groupUris)
        {
            _avatars = avatars;
            _emailAddresses = emailAddresses;
            _phoneNumbers = phoneNumbers;
            _organizations = organizations;
            _relationships = relationships;
            _groupUris = groupUris;
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

        public IEnumerable<string> Tags
        {
            get { return _tags.AsEnumerable(); }
        }

        public IEnumerable<Uri> GroupUris
        {
            get { return _groupUris; }
        }

        public void AddTag(string tag)
        {
            _tags.Add(tag);
        }
    }

    public interface IGoogleContactProfile : IContactProfile
    {
        IEnumerable<Uri> GroupUris { get; }
        void AddTag(string tag);
    }
}
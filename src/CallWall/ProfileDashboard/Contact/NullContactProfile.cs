using System;
using System.Collections.Generic;
using CallWall.Contract;
using CallWall.Contract.Contact;

namespace CallWall.ProfileDashboard.Contact
{
    public sealed class NullContactProfile : IContactProfile
    {
        public static readonly IContactProfile Instance = new NullContactProfile();

        private NullContactProfile()
        {}

        public string Title { get { return null; } }
        public string FullName { get { return null; } }
        public IEnumerable<Uri> Avatars { get { return null; } }
        public DateTime? DateOfBirth { get { return null; } }
        public IEnumerable<IContactAssociation> Organizations { get { return null; } }
        public IEnumerable<IContactAssociation> Relationships { get { return null; } }
        public IEnumerable<IContactAssociation> EmailAddresses { get { return null; } }
        public IEnumerable<IContactAssociation> PhoneNumbers { get { return null; } }
    }
}
using System;
using System.Collections.Generic;
using CallWall.Contract;

namespace CallWall.FakeProvider.Providers
{
    public sealed class GoogleContactProfile : IContactProfile
    {
        private static readonly IEnumerable<IContactAssociation> _noAssociations = new IContactAssociation[] { };

        /// <summary>
        /// How the user commonly references the contact e.g. Dan Rowe
        /// </summary>
        public string Title
        {
            get { return "Lee FakeCampbell"; }
        }

        /// <summary>
        /// The formal or full name of the contact e.g. Mr. Daniel Alex Rowe
        /// </summary>
        public string FullName
        {
            get { return "Mr. Lee Ryan Campbell"; }
        }

        /// <summary>
        /// Link to an image or avatar of the contact
        /// </summary>
        public IEnumerable<Uri> Avatars
        {
            get
            {
                return new[]
                             {
                                 new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Profile/Profile1.png"),
                                 new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Profile/Profile2.jpg"),
                                 new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Profile/Profile3.png"),
                             };
            }
        }

        /// <summary>
        /// The Date of birth for the contact. If the Year is unknown then it should be set to a value of 1.
        /// </summary>
        public DateTime? DateOfBirth
        {
            get { return new DateTime(1979, 12, 27); }
        }

        public IEnumerable<IContactAssociation> Organizations { get { return _noAssociations; } }

        public IEnumerable<IContactAssociation> Relationships { get { return _noAssociations; } }

        public IEnumerable<IContactAssociation> EmailAddresses { get { return _noAssociations; } }

        public IEnumerable<IContactAssociation> PhoneNumbers { get { return _noAssociations; } }
    }
}
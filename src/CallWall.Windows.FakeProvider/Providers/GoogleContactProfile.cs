using System;
using System.Collections.Generic;
using CallWall.Windows.Contract.Contact;

namespace CallWall.Windows.FakeProvider.Providers
{
    public sealed class GoogleContactProfile : IContactProfile
    {
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
                                 new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Profile/Profile1.png"),
                                 new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Profile/Profile2.jpg"),
                                 new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Profile/Profile3.png"),
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

        public IEnumerable<IContactAssociation> Organizations
        {
            get
            {
                return new IContactAssociation[]
                {
                    new ContactAssociation("Work", "RBC Capital Markets"), 
                    new ContactAssociation("Owner", "Campbell Consulting London Ltd"), 
                    new ContactAssociation("Volunteer", "VLCON - Very long charitable organization name"), 
                };
            }
        }

        public IEnumerable<IContactAssociation> Relationships
        {
            get
            {
                return new IContactAssociation[]
                {
                    new ContactAssociation("Wife", "Erynne Campbell"), 
                    new ContactAssociation("Brother", "Rhys Campbell"), 
                };
            }
        }

        public IEnumerable<IContactAssociation> EmailAddresses
        {
            get
            {
                return new IContactAssociation[]
                {
                    new ContactAssociation("Home", "Lee.Ryan.Campbell@gmail.com"), 
                    new ContactAssociation("Work", "Lee.Campbell@work.com"), 
                    new ContactAssociation("Home", "Lee.Ryan.Campbell@CampbellConsultingLondonLimited.com"), //Really long email to test formatting.
                };
            }
        }

        public IEnumerable<IContactAssociation> PhoneNumbers
        {
            get
            {
                return new IContactAssociation[]
                {
                    new ContactAssociation("Mobile", "+44 7 8277 43025"),
                    new ContactAssociation("Home", "+44 8 1234 1234"), 
                    new ContactAssociation("Work", "+44 8 43231 4321"), 
                };
            }
        }

        public IEnumerable<string> Tags
        {
            get
            {
                return new[]
                {
                    "Family",
                    "UK", 
                };
            }
        }

        private sealed class ContactAssociation : IContactAssociation
        {
            private readonly string _name;
            private readonly string _association;

            public ContactAssociation(string name, string association)
            {
                _name = name;
                _association = association;
            }

            public string Name { get { return _name; } }

            public string Association { get { return _association; } }
        }
    }
}
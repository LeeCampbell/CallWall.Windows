//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using JetBrains.Annotations;

//namespace CallWall.ProfileDashboard.ContactSummary
//{
//    public sealed class ContactSummaryAggregator : IContactSummary, INotifyPropertyChanged
//    {
//        #region Implementation of IContactSummary

//        private string _title;
//        private string _fullName;
//        private Uri _image;
//        private DateTime? _dateOfBirth;
//        private IEnumerable<IContactAssociation> _organizations;
//        private IEnumerable<IContactAssociation> _relationships;
//        private IEnumerable<IContactAssociation> _emailAddresses;
//        private IEnumerable<IContactAssociation> _phoneNumbers;

//        public ContactSummaryAggregator()
//        {
//            //Get identity activator
//            //Get Contact providers..
//        }

//        public string Title
//        {
//            get { return _title; }
//        }

//        public string FullName
//        {
//            get { return _fullName; }
//        }

//        public Uri Image
//        {
//            get { return _image; }
//        }

//        public DateTime? DateOfBirth
//        {
//            get { return _dateOfBirth; }
//        }

//        public IEnumerable<IContactAssociation> Organizations
//        {
//            get { return _organizations; }
//        }

//        public IEnumerable<IContactAssociation> Relationships
//        {
//            get { return _relationships; }
//        }

//        public IEnumerable<IContactAssociation> EmailAddresses
//        {
//            get { return _emailAddresses; }
//        }

//        public IEnumerable<IContactAssociation> PhoneNumbers
//        {
//            get { return _phoneNumbers; }
//        }

//        #endregion

//        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
//        private void OnPropertyChanged(string propertyName)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
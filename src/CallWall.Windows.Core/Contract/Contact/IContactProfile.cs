using System;
using System.Collections.Generic;

namespace CallWall.Windows.Contract.Contact
{
    public interface IContactProfile
    {
        /// <summary>
        /// How the user commonly references the contact e.g. Dan Rowe
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The formal or full name of the contact e.g. Mr. Daniel Alex Rowe
        /// </summary>
        string FullName { get; }

        //Full Name
        //GivenName
        //FamilyName

        /// <summary>
        /// Link to an image or avatar of the contact
        /// </summary>
        IEnumerable<Uri> Avatars { get; }

        /// <summary>
        /// The Date of birth for the contact. If the Year is unknown then it should be set to a value of 1.
        /// </summary>
        DateTime? DateOfBirth { get; }

        /// <summary>
        /// Any tags a.k.a groups that the contact belongs to. e.g. Friends, Family, Co-workers etc.
        /// </summary>
        IEnumerable<string> Tags { get; } 

        IEnumerable<IContactAssociation> Organizations { get; }

        IEnumerable<IContactAssociation> Relationships { get; }

        //EmailAddresses
        /*
         * Association e.g. Home/Work
         * Value e.g. lee@home.com
         * IsPrimary? Just put this one first?
         */
        IEnumerable<IContactAssociation> EmailAddresses { get; }

        //PhoneNumbers
        /*
         * Association : e.g. Home/work/mobile
         */
        IEnumerable<IContactAssociation> PhoneNumbers { get; }
    }
}

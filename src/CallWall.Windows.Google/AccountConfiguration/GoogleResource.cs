using System;
using System.Collections.ObjectModel;
using CallWall.Windows.Contract;

namespace CallWall.Windows.Google.AccountConfiguration
{
    public sealed class GoogleResource : IResourceScope
    {
        public static readonly GoogleResource Contacts;
        public static readonly GoogleResource Gmail;
        public static readonly GoogleResource Calendar;

        private static readonly ReadOnlyCollection<GoogleResource> _availableResourceScopes;

        private readonly string _name;
        private readonly Uri _image;
        private readonly Uri _resource;

        static GoogleResource()
        {
            Ensure.PackUriIsRegistered();
            Contacts = new GoogleResource("Contacts", "Contacts_48x48.png", new Uri(@"https://www.google.com/m8/feeds/"));
            Gmail = new GoogleResource("Email", "Email_48x48.png", new Uri(@"https://mail.google.com/"));
            Calendar = new GoogleResource("Calendar", "Calendar_48x48.png", null);
            _availableResourceScopes = new ReadOnlyCollection<GoogleResource>(new[]
                {
                    Contacts,
                    Gmail,
                    Calendar
                });
        }

        private GoogleResource(string name, string image, Uri resource)
        {
            _name = name;
            _image = new Uri(string.Format("pack://application:,,,/CallWall.Windows.Google;component/Images/{0}", image));
            _resource = resource;
        }

        public static ReadOnlyCollection<GoogleResource> AvailableResourceScopes
        {
            get { return _availableResourceScopes; }
        }

        public string Name { get { return _name; } }

        public Uri Resource { get { return _resource; } }

        public Uri Image { get { return _image; } }

        public override string ToString()
        {
            return string.Format("GoogleResource{{{0}}}", Name);
        }
    }
}
using System;
using System.Collections.ObjectModel;
using CallWall.Contract;

namespace CallWall.Google.AccountConfiguration
{
    public sealed class GoogleResource : IResourceScope
    {
        private readonly string _name;
        private readonly Uri _image;
        private readonly Uri _resource;

        static GoogleResource()
        {
            Ensure.PackUriIsRegistered();
        }

        private GoogleResource(string name, string image, Uri resource)
        {
            _name = name;
            _image = new Uri(string.Format("pack://application:,,,/CallWall.Google;component/Images/{0}", image));
            _resource = resource;
        }

        //Could go back to being a static list now that IsEnabled has been removed.-LC
        public static ReadOnlyCollection<GoogleResource> AvailableResourceScopes()
        {
            return new ReadOnlyCollection<GoogleResource>(new[]
                {
                    new GoogleResource("Contacts", "Contacts_48x48.png", new Uri(@"https://www.google.com/m8/feeds/")),
                    new GoogleResource("Email", "Email_48x48.png", new Uri(@"https://mail.google.com/")),
                    new GoogleResource("Calendar", "Calendar_48x48.png", null)
                });
        }

        public string Name { get { return _name; } }

        public Uri Resource { get { return _resource; } }

        public Uri Image { get { return _image; } }
    }
}
using System;
using CallWall.Windows.Contract;

namespace CallWall.Windows.Google.Providers.Gmail
{
    public sealed class GmailProviderDescription : IProviderDescription
    {
        public static readonly GmailProviderDescription Instance = new GmailProviderDescription();

        private GmailProviderDescription()
        { }

        public string Name
        {
            get { return "Gmail"; }
        }

        public Uri Image
        {
            get { return new Uri("pack://application:,,,/CallWall.Windows.Google;component/Images/Email_48x48.png"); }
        }
    }
}
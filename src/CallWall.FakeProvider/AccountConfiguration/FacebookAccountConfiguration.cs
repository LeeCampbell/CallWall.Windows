using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallWall.FakeProvider.AccountConfiguration
{
    internal class FacebookAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Facebook"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Facebook_64x64.png"); } }
        public object View { get { return "Facebook"; } } 
    }

    internal class MicrosoftAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Microsoft"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Microsoft_64x64.png"); } }
        public object View { get { return "Microsoft"; } } 
    }
    internal class LinkedInAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "LinkedIn"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/LinkedIn_64x64.png"); } }
        public object View { get { return "LinkedIn"; } }
    }
    internal class TwitterAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Twitter"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Twitter_64x64.png"); } }
        public object View { get { return "Twitter"; } }
    }
    internal class YahooAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Yahoo"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Yahoo_64x64.png"); } }
        public object View { get { return "Yahoo"; } }
    }
    internal class GithubAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Github"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Github_64x64.png"); } }
        public object View { get { return "Github"; } }
    }
}

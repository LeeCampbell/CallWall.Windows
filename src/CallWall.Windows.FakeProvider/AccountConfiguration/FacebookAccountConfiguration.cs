using System;
using System.ComponentModel;

namespace CallWall.Windows.FakeProvider.AccountConfiguration
{
    internal class FacebookAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Facebook"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Accounts/Facebook_64x64.png"); } }
        public object View { get { return "Facebook"; } } 
    }
}

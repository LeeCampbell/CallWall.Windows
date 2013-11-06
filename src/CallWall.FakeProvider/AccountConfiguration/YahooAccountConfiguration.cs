using System;
using System.ComponentModel;

namespace CallWall.FakeProvider.AccountConfiguration
{
    internal class YahooAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Yahoo"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Yahoo_64x64.png"); } }
        public object View { get { return "Yahoo"; } }
    }
}
using System;
using System.ComponentModel;

namespace CallWall.FakeProvider.AccountConfiguration
{
    internal class GoogleAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Google"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Google_64x64.png"); } }
        public object View { get { return "Google"; } }
    }
}
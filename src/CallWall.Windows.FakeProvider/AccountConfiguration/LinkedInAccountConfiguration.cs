using System;
using System.ComponentModel;

namespace CallWall.Windows.FakeProvider.AccountConfiguration
{
    internal class LinkedInAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "LinkedIn"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Accounts/LinkedIn_64x64.png"); } }
        public object View { get { return "LinkedIn"; } }
    }
}
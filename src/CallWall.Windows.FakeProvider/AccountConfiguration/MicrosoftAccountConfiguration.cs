using System;
using System.ComponentModel;

namespace CallWall.Windows.FakeProvider.AccountConfiguration
{
    internal class MicrosoftAccountConfiguration : IAccountConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }
        public string Name { get { return "Microsoft"; } }
        public Uri Image { get { return new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Accounts/Microsoft_64x64.png"); } }
        public object View { get { return "Microsoft"; } } 
    }
}
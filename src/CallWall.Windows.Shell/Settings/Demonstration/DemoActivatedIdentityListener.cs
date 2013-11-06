using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Windows.Contract;

namespace CallWall.Windows.Shell.Settings.Demonstration
{
    public sealed class DemoActivatedIdentityListener : IDemoProfileActivator
    {
        private readonly Subject<string> _activatedIdenties = new Subject<string>();

        public DemoActivatedIdentityListener()
        {
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public IObservable<IProfile> ProfileActivated()
        {
            return _activatedIdenties.Select(id => new Profile(new[] { new PersonalIdentifier("phone", id, DemoProviderDescription.Instance) }));
        }

        public void ActivateIdentity(string identity)
        {
            _activatedIdenties.OnNext(identity);
        }

        
        private sealed class DemoProviderDescription : IProviderDescription
        {
            public static readonly DemoProviderDescription Instance = new DemoProviderDescription();
            private DemoProviderDescription()
            { }

            public string Name { get { return "Test"; } }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Connectivity/Cloud_72x72.png"); }
            }
        }
    }
}
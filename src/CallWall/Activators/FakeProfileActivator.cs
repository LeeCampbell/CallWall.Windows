using System;
using System.Reactive.Linq;
using CallWall.Contract;

namespace CallWall.Activators
{
    public sealed class FakeProfileActivator : IProfileActivator
    {
        public bool IsEnabled { get; set; }

        public IObservable<IProfile> ProfileActivated()
        {
            return Observable.Return(new Profile(new[] { new PersonalIdentifier("email", "lee@mail.com", FakeProviderDescription.Instance), }));
        }

        private sealed class FakeProviderDescription : IProviderDescription
        {
            public static readonly FakeProviderDescription Instance = new FakeProviderDescription();
            private  FakeProviderDescription()
            {}

            public string Name { get { return "Test"; } }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Connectivity/Cloud_72x72.png"); }
            }
        }
    }
}
using System;
using System.Reactive.Linq;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Contact;

namespace CallWall.Windows.FakeProvider.Providers
{
    public sealed class FakeGoogleContactQueryProvider : IContactQueryProvider
    {
        public IObservable<IContactProfile> LoadContact(IProfile activeProfile)
        {
            return Observable.Return(new GoogleContactProfile()).Delay(TimeSpan.FromSeconds(3));
        }
    }
}

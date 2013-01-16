using System;
using System.Reactive.Linq;
using CallWall.Contract;

namespace CallWall.FakeProvider.Providers
{
    public sealed class FakeGoogleContactQueryProvider : IContactQueryProvider
    {
        public IObservable<IContactProfile> Search(IProfile activeProfile)
        {
            return Observable.Return(new GoogleContactProfile());
        }
    }
}

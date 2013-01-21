﻿using System;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;

namespace CallWall.FakeProvider.Providers
{
    public sealed class FakeGoogleContactQueryProvider : IContactQueryProvider
    {
        public IObservable<IContactProfile> LoadContact(IProfile activeProfile)
        {
            return Observable.Return(new GoogleContactProfile());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Contract;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileActivator : IProfileActivator
    {
        #region Implementation of IProfileActivator

        public IObservable<IProfile> ProfileActivated()
        {
            return Observable.Return(new FakeProfile(new[]
                                                         {
                                                             new FakePersonalIdentifier("email", "lee@mail.com"),
                                                         }));
        }

        #endregion

        private sealed class FakeProfile : IProfile
        {
            private readonly IList<IPersonalIdentifier> _identifiers;

            public FakeProfile(IEnumerable<IPersonalIdentifier> identifiers)
            {
                _identifiers = identifiers.ToList();
            }

            public IList<IPersonalIdentifier> Identifiers
            {
                get { return _identifiers; }
            }
        }
        private sealed class FakePersonalIdentifier : IPersonalIdentifier
        {
            private readonly IProviderDescription _provider;
            private readonly string _identifierType;
            private readonly string _value;

            public FakePersonalIdentifier(string identifierType, string value)
            {
                _provider = new FakeProviderDescription();
                _identifierType = identifierType;
                _value = value;
            }

            public IProviderDescription Provider
            {
                get { return _provider; }
            }

            public string IdentifierType
            {
                get { return _identifierType; }
            }

            public string Value
            {
                get { return _value; }
            }
        }
        private sealed class FakeProviderDescription : IProviderDescription
        {
            public string Name
            {
                get { return "Test"; }
            }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Connectivity/Cloud_72x72.png"); }
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace CallWall.Contract
{
    public interface IIdentityProvider
    {
        IObservable<IProfile> FindProfile(IList<string> identityKeys);
    }
}
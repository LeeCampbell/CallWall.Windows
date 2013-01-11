using System;
using System.Collections.Generic;

namespace CallWall
{
    public interface IActivatedIdentityListener
    {
        IObservable<IList<string>> IdentitiesActivated();
    }
}
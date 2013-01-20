using System;

namespace CallWall.Contract.Contact
{
    public interface IContactQueryProvider
    {
        IObservable<IContactProfile> Search(IProfile activeProfile);
    }
}
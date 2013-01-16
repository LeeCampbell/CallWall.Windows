using System;

namespace CallWall.Contract
{
    public interface IContactQueryProvider
    {
        IObservable<IContactProfile> Search(IProfile activeProfile);
    }
}
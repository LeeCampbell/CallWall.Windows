using System;

namespace CallWall.Windows.Contract.Contact
{
    public interface IContactQueryProvider
    {
        IObservable<IContactProfile> LoadContact(IProfile activeProfile);
    }
}
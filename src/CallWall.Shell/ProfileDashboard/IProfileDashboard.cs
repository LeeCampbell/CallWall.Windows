using System;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.ProfileDashboard.Communication;
using CallWall.ProfileDashboard.Pictures;

namespace CallWall.ProfileDashboard
{
    public interface IProfileDashboard : IDisposable
    {
        void Load(IProfile profile);

        string ActivatedIdentity { get; }

        IObservable<IContactProfile> Contact { get; }

        IObservable<Message> Messages { get; }

        IObservable<Album> PictureAlbums { get; } 
    }
}
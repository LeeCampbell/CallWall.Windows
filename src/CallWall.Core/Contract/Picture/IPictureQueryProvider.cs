using System;

namespace CallWall.Contract.Picture
{
    public interface IPictureQueryProvider
    {
        IObservable<IAlbum> LoadPictures(IProfile activeProfile);
    }
}

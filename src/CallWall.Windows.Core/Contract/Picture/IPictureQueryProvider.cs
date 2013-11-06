using System;

namespace CallWall.Windows.Contract.Picture
{
    public interface IPictureQueryProvider
    {
        IObservable<IAlbum> LoadPictures(IProfile activeProfile);
    }
}

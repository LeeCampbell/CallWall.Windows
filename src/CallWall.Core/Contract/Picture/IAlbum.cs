using System.Collections.Generic;

namespace CallWall.Contract.Picture
{
    public interface IAlbum
    {
        string Name { get; }

        IEnumerable<IPicture> Pictures { get; }
    }
}
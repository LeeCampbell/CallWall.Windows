using System.Collections.Generic;

namespace CallWall.Windows.Contract.Picture
{
    public interface IAlbum
    {
        string Name { get; }

        IEnumerable<IPicture> Pictures { get; }

        IProviderDescription Provider { get; }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Picture;

namespace CallWall.Windows.Shell.ProfileDashboard.Pictures
{
    public sealed class Album
    {
        private readonly IAlbum _album;
        private readonly ReadOnlyCollection<IPicture> _pictures;
        private readonly DateTimeOffset _lastUpdate;

        public Album(IAlbum album)
        {
            _album = album;
            _pictures = new ReadOnlyCollection<IPicture>(album.Pictures.ToArray());
            _lastUpdate = _pictures.Max(p => p.Timestamp);
        }

        public string Name
        {
            get { return _album.Name; }
        }

        public ReadOnlyCollection<IPicture> Pictures
        {
            get { return _pictures; }
        }

        public DateTimeOffset LastUpdate
        {
            get { return _lastUpdate; }
        }

        public IProviderDescription Provider
        {
            get { return _album.Provider; }
        }
    }
}

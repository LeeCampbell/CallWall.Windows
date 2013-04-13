using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Picture;

namespace CallWall.ProfileDashboard.Pictures
{
    public sealed class PictureQueryAggregator : IPictureQueryAggregator
    {
        private readonly IEnumerable<IPictureQueryProvider> _pictureQueryProviders;

        public PictureQueryAggregator(IEnumerable<IPictureQueryProvider> pictureQueryProviders)
        {
            
            _pictureQueryProviders = pictureQueryProviders;
        }

        public IObservable<Album> Search(IProfile activeProfile)
        {
            return from provider in _pictureQueryProviders.ToObservable()
                   from album in provider.LoadPictures(activeProfile)
                   select new Album(album);
        }
    }
}
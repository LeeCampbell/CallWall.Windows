using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Picture;

namespace CallWall.FakeProvider.Providers
{
    public sealed class FacebookPictureQueryProvider : IPictureQueryProvider
    {
        public IObservable<IAlbum> LoadPictures(IProfile activeProfile)
        {
            return Observable.Create<IAlbum>(
                o =>
                    {
                        var snowSwim = new Album
                                           {
                                               Name = "Snow Swim",
                                               Pictures =
                                                   {
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/a.jpg")},
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/b.jpg")},
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/c.jpg")},
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/d.jpg")},
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/e.jpg")},
                                                       new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-3), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/SnowSwim/f.jpg")},
                                                   }
                                           };
                        o.OnNext(snowSwim);


                        return Disposable.Empty;
                    });
        }

        private sealed class Album : IAlbum
        {
            public Album()
            {
                Pictures = new List<IPicture>();
            }
            public string Name { get; set; }

            public List<IPicture> Pictures { get; private set; }

            IEnumerable<IPicture> IAlbum.Pictures { get { return Pictures; } }

            IProviderDescription IAlbum.Provider { get { return FacebookProvider.Instance; } }
        }

        private sealed class Picture : IPicture
        {
            public DateTimeOffset Timestamp { get; set; }
            public Uri Source { get; set; }
            public string Caption { get; set; }
        }

        private sealed class FacebookProvider : IProviderDescription
        {
            public static readonly IProviderDescription Instance = new FacebookProvider();

            private FacebookProvider()
            { }
            public string Name { get { return "Facebook"; } }

            public Uri Image { get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Accounts/Facebook_64x64.png"); } }
        }
    }
}
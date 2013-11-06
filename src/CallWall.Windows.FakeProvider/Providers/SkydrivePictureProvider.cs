using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Picture;

namespace CallWall.Windows.FakeProvider.Providers
{
    public sealed class SkydrivePictureQueryProvider : IPictureQueryProvider
    {
        public IObservable<IAlbum> LoadPictures(IProfile activeProfile)
        {
            return Observable.Create<IAlbum>(
                o =>
                {
                    var xmasFood = new Album
                    {
                        Name = "Xmas food 2012",
                        Pictures =
                            {
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-10), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/a.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/b.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/c.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/d.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/e.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/XmasFood/f.jpg")},
                            }
                    };
                    o.OnNext(xmasFood);

                    var xmasDay = new Album
                    {
                        Name = "Xmas day",
                        Pictures =
                            {
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/Xmas/a.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/Xmas/b.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Pictures/Xmas/c.jpg")},
                            }
                    };
                    o.OnNext(xmasDay);
                    o.OnCompleted();
                    return Disposable.Empty;
                })
                .Zip(Observable.Interval(TimeSpan.FromMilliseconds(500)), (album, l) => album);
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

            IProviderDescription IAlbum.Provider { get { return SkyDriveProvider.Instance; } }
        }

        private sealed class Picture : IPicture
        {
            public DateTimeOffset Timestamp { get; set; }
            public Uri Source { get; set; }
            public string Caption { get; set; }
        }

        private sealed class SkyDriveProvider : IProviderDescription
        {
            public static readonly IProviderDescription Instance = new SkyDriveProvider();

            private SkyDriveProvider()
            { }
            public string Name { get { return "SkyDrive"; } }

            public Uri Image { get { return new Uri("pack://application:,,,/CallWall.Windows.FakeProvider;component/Images/Accounts/Microsoft_64x64.png"); } }
        }
    }
}

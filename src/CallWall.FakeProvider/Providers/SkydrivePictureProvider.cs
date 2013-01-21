using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Picture;

namespace CallWall.FakeProvider.Providers
{
    public sealed class SkydrivePictureQueryProvider : IPictureQueryProvider
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

                    var xmasFood = new Album
                    {
                        Name = "Xmas food 2012",
                        Pictures =
                            {
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-10), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/a.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/b.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/c.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/d.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/e.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-11), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/XmasFood/f.jpg")},
                            }
                    };
                    o.OnNext(xmasFood);

                    var xmasDay = new Album
                    {
                        Name = "Xmas day",
                        Pictures =
                            {
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/Xmas/a.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/Xmas/b.jpg")},
                                new Picture{ Timestamp = DateTimeOffset.Now.AddDays(-15), Source= new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Pictures/Xmas/c.jpg")},
                            }
                    };
                    o.OnNext(xmasDay);
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

            public List<IPicture> Pictures { get; set; }

            IEnumerable<IPicture> IAlbum.Pictures
            {
                get { return Pictures; }
            }
        }

        private sealed class Picture : IPicture
        {
            public DateTimeOffset Timestamp { get; set; }
            public Uri Source { get; set; }
            public string Caption { get; set; }
        }
    }
}

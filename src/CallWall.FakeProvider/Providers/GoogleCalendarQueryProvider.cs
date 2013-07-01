using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Calendar;

namespace CallWall.FakeProvider.Providers
{
    public sealed class GoogleCalendarQueryProvider : ICalendarQueryProvider
    {
        public IObservable<ICalendarEvent> LoadCalendar(IProfile activeProfile)
        {
            return Observable.Create<ICalendarEvent>(o =>
                {

                    var x = new CalendarEvent
                        {
                            Start = DateTimeOffset.UtcNow.Date.AddDays(3).AddHours(10),
                            End = DateTimeOffset.UtcNow.Date.AddDays(3).AddHours(12),
                            Name = "Lunch with Lee",
                            Provider = GoogleCalendarProvider.Instance
                        };
                    o.OnNext(x);

                    x = new CalendarEvent
                    {
                        Start = DateTimeOffset.UtcNow.Date.AddDays(1).AddHours(18),
                        End = DateTimeOffset.UtcNow.Date.AddDays(1).AddHours(19.5),
                        Name = "Training",
                        Provider = GoogleCalendarProvider.Instance
                    };
                    o.OnNext(x);

                    x = new CalendarEvent
                    {
                        Start = DateTimeOffset.UtcNow.Date.AddHours(18),
                        End = DateTimeOffset.UtcNow.Date.AddHours(19.5),
                        Name = "Document review",
                        Provider = GoogleCalendarProvider.Instance
                    };
                    o.OnNext(x);

                    x = new CalendarEvent
                    {
                        Start = DateTimeOffset.UtcNow.Date.AddDays(-10).AddHours(18),
                        End = DateTimeOffset.UtcNow.Date.AddDays(-10).AddHours(19.5),
                        Name = "Document design session",
                        Provider = GoogleCalendarProvider.Instance
                    };
                    o.OnNext(x);
                    o.OnCompleted();
                    return Disposable.Empty;
                });
        }

        private sealed class GoogleCalendarProvider : IProviderDescription
        {
            public static readonly GoogleCalendarProvider Instance = new GoogleCalendarProvider();

            private GoogleCalendarProvider()
            { }

            public string Name
            {
                get { return "Google Calendar"; }
            }

            public Uri Image
            {
                //TODO:Create a Calendar image -LC
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Communication/Gmail_128x128.png"); }
            }
        }

        private sealed class CalendarEvent : ICalendarEvent
        {
            
            public DateTimeOffset Start { get;  set; }
            public DateTimeOffset End { get;  set; }
            public string Name { get;  set; }
            public string Description { get;  set; }
            public IProviderDescription Provider { get;  set; }
        }
    }


}

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.FakeProvider.Providers
{
    public sealed class TwitterCommunicationQueryProvider : ICommunicationQueryProvider
    {
        public IObservable<IMessage> Messages(IProfile activeProfile)
        {
            return Observable.Create<IMessage>(
                o =>
                    {
                        o.OnNext(new Tweet(DateTimeOffset.Now.AddDays(-3), MessageDirection.Inbound, "Great game, great win."));
                        o.OnNext(new Tweet(DateTimeOffset.Now.AddDays(-3).AddHours(-2), MessageDirection.Inbound, "Good luck for the big game #COYQ"));
                        o.OnNext(new Tweet(DateTimeOffset.Now.AddDays(-10), MessageDirection.Inbound, "New blog post out today #RxNet"));
                        o.OnNext(new Tweet(DateTimeOffset.Now.AddDays(-10), MessageDirection.Inbound, "IntroToRx.com has been updated with new content. Async/Await, testing Hot and Cold observables and a new cookbook section. Retweet and share the love! #RxNet"));
                        return Disposable.Empty;
                    });

        }

        private sealed class Tweet : IMessage
        {
            private readonly DateTimeOffset _timestamp;
            private readonly MessageDirection _direction;
            private readonly string _subject;

            public Tweet(DateTimeOffset timestamp, MessageDirection direction, string subject)
            {
                _timestamp = timestamp;
                _direction = direction;
                _subject = subject;
            }

            public DateTimeOffset Timestamp { get { return _timestamp; } }

            public MessageDirection Direction { get { return _direction; } }

            public string Subject { get { return _subject; } }

            public string Content { get { return null; } }

            public IProviderDescription Provider { get { return TwitterProvider.Instance; } }
        }

        private sealed class TwitterProvider : IProviderDescription
        {
            public static readonly TwitterProvider Instance = new TwitterProvider();

            private TwitterProvider()
            { }

            public string Name
            {
                get { return "Twitter"; }
            }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Communication/Twitter_48x48.png"); }
            }
        }
    }
}
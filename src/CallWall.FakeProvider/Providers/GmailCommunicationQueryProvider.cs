using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.FakeProvider.Providers
{
    public class GmailCommunicationQueryProvider : ICommunicationQueryProvider
    {
        public IObservable<IMessage> LoadMessages(IProfile activeProfile)
        {
            return Observable.Create<IMessage>(
                o =>
                {
                    o.OnNext(new GmailEmail(DateTimeOffset.Now.AddDays(-1), MessageDirection.Inbound, "Pricing a Cross example", "Here is that sample we were talking about the other day. If you want to be able to price a cross, first you need to gets the component..."));
                    o.OnNext(new GmailEmail(DateTimeOffset.Now.AddDays(-4), MessageDirection.Outbound, "Oz Travel plans?", "Are you guys planning a trip to Ozzie yet?"));
                    o.OnNext(new GmailEmail(DateTimeOffset.Now.AddDays(-10), MessageDirection.Outbound, "Happy Birthday", "Quick happy birthday note."));
                    return Disposable.Empty;
                });

        }

        private sealed class GmailEmail : IMessage
        {
            private readonly DateTimeOffset _timestamp;
            private readonly MessageDirection _direction;
            private readonly string _subject;
            private readonly string _content;

            public GmailEmail(DateTimeOffset timestamp, MessageDirection direction, string subject, string content)
            {
                _timestamp = timestamp;
                _direction = direction;
                _subject = subject;
                _content = content;
            }

            public DateTimeOffset Timestamp { get { return _timestamp; } }

            public MessageDirection Direction { get { return _direction; } }

            public string Subject { get { return _subject; } }

            public string Content { get { return _content; } }

            public IProviderDescription Provider { get { return GmailProvider.Instance; } }
        }

        private sealed class GmailProvider : IProviderDescription
        {
            public static readonly GmailProvider Instance = new GmailProvider();

            private GmailProvider()
            { }

            public string Name
            {
                get { return "Gmail"; }
            }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Communication/Gmail_128x128.png"); }
            }
        }
    }
}

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.FakeProvider.Providers
{
    public sealed class SmsCommunicationQueryProvider : ICommunicationQueryProvider
    {
        public IObservable<IMessage> LoadMessages(IProfile activeProfile)
        {
            return Observable.Create<IMessage>(
                o =>
                    {
                        o.OnNext(new SMS(DateTimeOffset.Now.AddHours(-1), MessageDirection.Inbound, "On my way"));
                        o.OnNext(new SMS(DateTimeOffset.Now.AddHours(-1.5), MessageDirection.Outbound, "Dude, where are you."));
                        o.OnNext(new SMS(DateTimeOffset.Now.AddDays(-10), MessageDirection.Outbound, "Happy Birthday"));
                        o.OnCompleted();
                        return Disposable.Empty;
                    });

        }

        private sealed class SMS : IMessage
        {
            private readonly DateTimeOffset _timestamp;
            private readonly MessageDirection _direction;
            private readonly string _subject;

            public SMS(DateTimeOffset timestamp, MessageDirection direction, string subject)
            {
                _timestamp = timestamp;
                _direction = direction;
                _subject = subject;
            }

            public DateTimeOffset Timestamp { get { return _timestamp; } }

            public MessageDirection Direction { get { return _direction; } }

            public string Subject { get { return _subject; } }

            public string Content { get { return null; } }

            public IProviderDescription Provider { get { return SmsProvider.Instance; } }
        }

        private sealed class SmsProvider : IProviderDescription
        {
            public static readonly SmsProvider Instance = new SmsProvider();

            private SmsProvider()
            { }

            public string Name
            {
                get { return "SMS"; }
            }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Communication/SMS_48x48.png"); }
            }
        }
    }
}
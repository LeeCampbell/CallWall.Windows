using System;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail
{
    public sealed class GmailEmail : IMessage
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

        public IProviderDescription Provider { get { return GmailProviderDescription.Instance; } }

        public MessageType MessageType { get { return MessageType.Email; } }

        public override string ToString()
        {
            return string.Format("Gmail {{{0:o} {1} Subject:'{2}'}}", Timestamp, Direction, Subject.TrimTo(50));
        }
    }
}
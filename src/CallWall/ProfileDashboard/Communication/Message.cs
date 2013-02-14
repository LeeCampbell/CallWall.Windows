using System;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.ProfileDashboard.Communication
{
    public sealed class Message : IMessage
    {
        private readonly IMessage _data;

        public Message(IMessage data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _data = data;
        }

        public DateTimeOffset Timestamp
        {
            get { return _data.Timestamp; }
        }

        public MessageDirection Direction
        {
            get { return _data.Direction; }
        }

        public string Subject
        {
            get { return _data.Subject; }
        }

        public string Content
        {
            get { return _data.Content; }
        }

        public IProviderDescription Provider
        {
            get { return _data.Provider; }
        }

        public override string ToString()
        {
            return string.Format("Message{{Subject='{0}'}}", this.Subject);
        }
    }
}

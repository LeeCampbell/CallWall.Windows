using System;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.ProfileDashboard.Communication
{
    public sealed class MessageViewModel : IMessage
    {
        private readonly IMessage _data;

        public MessageViewModel(IMessage data)
        {
            _data = data;
        }

        public DateTimeOffset Timestamp { get { return _data.Timestamp; } }

        public MessageDirection Direction { get { return _data.Direction; } }

        public string Subject { get { return _data.Subject; } }

        public string Content { get { return _data.Content; } }

        public IProviderDescription Provider { get { return _data.Provider; } }
    }

    //public sealed class CommunicationViewModel
    //{
    //    private readonly ObservableCollection<IMessage> _messages = new ObservableCollection<IMessage>();
    //    private readonly ReadOnlyObservableCollection<IMessage> _roMessages;

    //    public CommunicationViewModel()
    //    {
    //        _roMessages = new ReadOnlyObservableCollection<IMessage>(_messages);
    //    }

    //    public ReadOnlyObservableCollection<IMessage> Messages
    //    {
    //        get { return _roMessages; }
    //    }
    //}
}

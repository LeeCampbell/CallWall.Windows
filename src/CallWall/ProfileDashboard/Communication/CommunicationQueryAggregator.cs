using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Communication;

namespace CallWall.ProfileDashboard.Communication
{
    public sealed class CommunicationQueryAggregator : ICommunicationQueryAggregator
    {
        private readonly IEnumerable<ICommunicationQueryProvider> _communicationQueryProviders;

        public CommunicationQueryAggregator(IEnumerable<ICommunicationQueryProvider> communicationQueryProviders)
        {
            _communicationQueryProviders = communicationQueryProviders;
        }

        public IObservable<Message> Search(IProfile activeProfile)
        {
            return from provider in _communicationQueryProviders.ToObservable()
                   from message in provider.LoadMessages(activeProfile)
                   select new Message(message);
        }
    }
}

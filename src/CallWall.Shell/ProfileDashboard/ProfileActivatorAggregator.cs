using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CallWall.Contract;

namespace CallWall.ProfileDashboard
{
    public sealed class ProfileActivatorAggregator : IProfileActivatorAggregator
    {
        private readonly IEnumerable<IProfileActivator> _profileActivators;

        public ProfileActivatorAggregator(IEnumerable<IProfileActivator> profileActivators)
        {
            _profileActivators = profileActivators;
        }

        public IObservable<IProfile> ProfileActivated()
        {
            return from activator in _profileActivators.ToObservable()
                   from activeProfile in activator.ProfileActivated()
                   select activeProfile;
        }
    }
}
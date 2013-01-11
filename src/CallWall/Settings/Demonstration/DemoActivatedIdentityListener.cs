using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CallWall.Settings.Demonstration
{
    public sealed class DemoActivatedIdentityListener : IDemoActivatedIdentityListener
    {
        private readonly Subject<string> _activatedIdenties = new Subject<string>();

        public IObservable<IList<string>> IdentitiesActivated()
        {
            return _activatedIdenties.Select(id => new[] {id});
        }

        public void ActivateIdentity(string identity)
        {
            _activatedIdenties.OnNext(identity);
        }
    }
}
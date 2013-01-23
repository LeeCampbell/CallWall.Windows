using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CallWall.Contract
{
    public class Profile : IProfile
    {
        private readonly ReadOnlyCollection<IPersonalIdentifier> _identifiers;

        public Profile(IEnumerable<IPersonalIdentifier> identifiers)
        {
            _identifiers = new ReadOnlyCollection<IPersonalIdentifier>(identifiers.ToArray());
        }

        public IList<IPersonalIdentifier> Identifiers { get { return _identifiers; } }
    }
}
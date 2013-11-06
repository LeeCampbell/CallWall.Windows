using System.Collections.Generic;

namespace CallWall.Windows.Contract
{
    public interface IProfile
    {
        IList<IPersonalIdentifier> Identifiers { get; }
    }
}
using System.Collections.Generic;

namespace CallWall.Contract
{
    public interface IProfile
    {
        IList<IPersonalIdentifier> Identifiers { get; }
    }
}
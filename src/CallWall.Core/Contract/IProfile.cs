using System.Collections.Generic;

namespace CallWall.Contract
{
    public interface IProfile
    {
        //IPersonalIdentifier Activation { get; } i.e. incoming call from +44 8 77665544
        IList<IPersonalIdentifier> Identifiers { get; }
    }
}
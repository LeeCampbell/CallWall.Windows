using System;
using CallWall.Contract;

namespace CallWall.Activators
{
    public interface IProfileActivator
    {
        bool IsEnabled { get; set; }//Could potentially (dis)Connect the underlying
        IObservable<IProfile> ProfileActivated();
    }
}
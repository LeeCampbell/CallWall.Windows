using System;

namespace CallWall.Windows.Contract
{
    public interface IProfileActivator
    {
        bool IsEnabled { get; set; }//Could potentially (dis)Connect the underlying
        IObservable<IProfile> ProfileActivated();
    }
}
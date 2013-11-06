using System;

namespace CallWall.Windows.Shell.Welcome
{
    public interface IWelcomeStep1View
    {
        event EventHandler NextView;
    }
}
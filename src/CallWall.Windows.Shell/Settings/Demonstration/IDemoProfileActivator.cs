using CallWall.Windows.Contract;

namespace CallWall.Windows.Shell.Settings.Demonstration
{
    public interface IDemoProfileActivator : IProfileActivator
    {
        void ActivateIdentity(string identity);
    }
}
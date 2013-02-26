using CallWall.Activators;

namespace CallWall.Settings.Demonstration
{
    public interface IDemoProfileActivator : IProfileActivator
    {
        void ActivateIdentity(string identity);
    }
}
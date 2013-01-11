namespace CallWall.Settings.Demonstration
{
    public interface IDemoActivatedIdentityListener : IActivatedIdentityListener
    {
        void ActivateIdentity(string identity);
    }
}
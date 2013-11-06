namespace CallWall.Windows.Google.Authorization.Login
{
    public interface IGoogleLoginView : Microsoft.Practices.Prism.IActiveAware
    {
        GoogleLoginViewModel ViewModel { get; }
    }
}

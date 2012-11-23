using System.Windows;

namespace CallWall
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _bootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.Dispose();
            base.OnExit(e);
        }
    }
}

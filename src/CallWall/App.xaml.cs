using System;
using System.Windows;
using System.Windows.Threading;

namespace CallWall
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper();

        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            
        }

        

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

        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (_bootstrapper == null) return;
            _bootstrapper.LogFailure("Application.DispatcherUnhandledException", e);
        }

        void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (_bootstrapper == null) return;
            _bootstrapper.LogFailure("AppDomain.UnhandledException", e);
        }
    }
}

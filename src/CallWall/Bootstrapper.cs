using CallWall.PrismExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Windows;

namespace CallWall
{
    public sealed class Bootstrapper : UnityBootstrapper, IDisposable
    {
        private readonly LoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public Bootstrapper()
        {
            Thread.CurrentThread.Name = "UI";
            _loggerFactory = new LoggerFactory();
            _logger = _loggerFactory.CreateLogger();
            _logger.Info("-------------------------------------------------------------------------------");
            _logger.Info("Starting application");
        }

        protected override Microsoft.Practices.Prism.Logging.ILoggerFacade CreateLogger()
        {
            return (Microsoft.Practices.Prism.Logging.ILoggerFacade)_logger;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog.Add<HostModule>();
            ModuleCatalog.Add<Settings.SettingsModule>();
            ModuleCatalog.Add<Welcome.WelcomeModule>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.AddNewExtension<GenericSupportExtension>();
            Container.RegisterInstance<ILoggerFactory>(_loggerFactory);
        }

        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
        }

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            return base.ConfigureRegionAdapterMappings();
        }

        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            return base.ConfigureDefaultRegionBehaviors();
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }

        protected override DependencyObject CreateShell()
        {
            var shell = new MainWindow();

            Application.Current.MainWindow = shell;
            shell.Show();
            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}

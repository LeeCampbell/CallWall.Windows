using CallWall.FakeProvider;
using CallWall.Logging;
using CallWall.PrismExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

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
            
            ModuleCatalog.Add<FakeModule>();

            ModuleCatalog.Add<ProfileDashboard.DashboardModule>();
            ModuleCatalog.Add<Welcome.WelcomeModule>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.AddNewExtension<GenericSupportExtension>();
            Container.RegisterInstance<ILoggerFactory>(_loggerFactory);
        }

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var baseMappings = base.ConfigureRegionAdapterMappings();
            baseMappings.RegisterMapping(typeof(System.Windows.Controls.Accordion), ServiceLocator.Current.GetInstance<AccordionRegionAdapter>());


            var windowStyle = (Style)App.Current.FindResource("WindowRegionStyle");
            RegionPopupBehaviors.RegisterNewWindowRegion(RegionNames.WindowRegion, windowStyle);

            return baseMappings;
        }

        //protected override void ConfigureServiceLocator()
        //{
        //    base.ConfigureServiceLocator();
        //}

        //protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    return base.ConfigureDefaultRegionBehaviors();
        //}

        //protected override void RegisterFrameworkExceptionTypes()
        //{
        //    base.RegisterFrameworkExceptionTypes();
        //}

        //protected override void InitializeShell()
        //{
        //    base.InitializeShell();
        //}

        //protected override void InitializeModules()
        //{
        //    base.InitializeModules();
        //}

        protected override DependencyObject CreateShell()
        {
            //var shell = new MainWindow();

            //Application.Current.MainWindow = shell;
            //shell.Show();
            //return shell;
            
            
            //Yeah pretty weird huh, this app doesn't have a shell. Technically it is a service that can show visuals (which end up being windows.) However closing a window doesn't indicate the end of the process. -LC
            return null;
        }

        public void LogFailure(string source, DispatcherUnhandledExceptionEventArgs args)
        {
            _logger.Fatal(args.Exception, "An unhandled exception occurred on the Dispatcher. args.Handled={0}", args.Handled);
        }

        public void LogFailure(string source, UnhandledExceptionEventArgs args)
        {
            _logger.Fatal("An unhandled exception occurred on the Dispatcher. args.IsTerminating={0}\r\nargs.ExceptionObject={1}", args.IsTerminating, args.ExceptionObject);
        }

        public void Dispose()
        {
            _logger.Debug("Bootstrapper disposing");
            Container.Dispose();
            _logger.Debug("Container disposed");
            _logger.Info("Application shutting down");
        }
    }
}

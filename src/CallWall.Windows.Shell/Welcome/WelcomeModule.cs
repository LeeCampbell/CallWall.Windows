using System;
using CallWall.Windows.Contract;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Windows.Shell.Welcome
{
    public sealed class WelcomeModule : IModule
    {
        private readonly ITypeRegistry _typeRegistry;
        private readonly Func<IWelcomeController> _welcomeControllerFactory;

        public WelcomeModule(ITypeRegistry typeRegistry, Func<IWelcomeController> welcomeControllerFactory)
        {
            _typeRegistry = typeRegistry;
            _welcomeControllerFactory = welcomeControllerFactory;
        }

        public void Initialize()
        {
            _typeRegistry.RegisterTypeAsTransient<IWelcomeController, WelcomeController>();
            _typeRegistry.RegisterTypeAsTransient<IWelcomeView, WelcomeView>();
            _typeRegistry.RegisterTypeAsTransient<IWelcomeStep1View, WelcomeStep1View>();

            var welcomeController = _welcomeControllerFactory();
            welcomeController.Start();
        }
    }
}

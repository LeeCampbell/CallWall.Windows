using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Welcome
{
    public sealed class WelcomeModule : IModule
    {
        private readonly IUnityContainer _container;

        public WelcomeModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IWelcomeController, WelcomeController>(new TransientLifetimeManager());
            _container.RegisterType<IWelcomeView, WelcomeView>(new TransientLifetimeManager());
            _container.RegisterType<IWelcomeStep1View, WelcomeStep1View>(new TransientLifetimeManager());

            var welcomeController = _container.Resolve<IWelcomeController>();
            welcomeController.Start();
        }
    }
}

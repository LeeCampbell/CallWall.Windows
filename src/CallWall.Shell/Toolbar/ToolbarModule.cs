using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Toolbar
{
    public sealed class ToolbarModule : IModule
    {
        private readonly IUnityContainer _container;
        private IToolbarController _toolbarController;

        public ToolbarModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IToolbarController, ToolbarController>(new ContainerControlledLifetimeManager());
            _toolbarController = _container.Resolve<IToolbarController>();
            _toolbarController.Start();
        }
    }
}
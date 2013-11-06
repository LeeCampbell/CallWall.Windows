using System;
using CallWall.Windows.Contract;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Toolbar
{
    public sealed class ToolbarModule : IModule, IDisposable
    {
        private readonly ITypeRegistry _typeRegistry;
        private readonly Func<IToolbarController> _toolbarControllerFactory;
        private IToolbarController _toolbarController;

        public ToolbarModule(ITypeRegistry typeRegistry, Func<IToolbarController> toolbarControllerFactory)
        {
            _typeRegistry = typeRegistry;
            _toolbarControllerFactory = toolbarControllerFactory;
        }

        public void Initialize()
        {
            _typeRegistry.RegisterTypeAsSingleton<IToolbarController, ToolbarController>();

            _toolbarController = _toolbarControllerFactory();
            _toolbarController.Start();
        }

        public void Dispose()
        {
            using (_toolbarController)
            {}
        }
    }
}
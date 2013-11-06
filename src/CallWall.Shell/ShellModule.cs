using CallWall.ProfileDashboard;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Shell
{
    public sealed class ShellModule : IModule
    {
        private readonly IUnityContainer _container;
        

        public ShellModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IProfileActivatorAggregator, ProfileActivatorAggregator>(new ContainerControlledLifetimeManager());
        }
    }
}

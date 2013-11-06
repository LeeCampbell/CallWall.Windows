using CallWall.Contract;
using CallWall.ProfileDashboard;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Shell
{
    public sealed class ShellModule : IModule
    {
        private readonly ITypeRegistry _typeRegistry;
        

        public ShellModule(ITypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
        }

        public void Initialize()
        {
            _typeRegistry.RegisterTypeAsSingleton<IProfileActivatorAggregator, ProfileActivatorAggregator>();
        }
    }
}

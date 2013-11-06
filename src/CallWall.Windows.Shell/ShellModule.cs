using CallWall.Windows.Contract;
using CallWall.Windows.Shell.ProfileDashboard;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Windows.Shell
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

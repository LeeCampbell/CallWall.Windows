using Microsoft.Practices.Prism.Modularity;

namespace CallWall.PrismExtensions
{
    public static class ModuleCatalogExtensions
    {
        public static void Add<T>(this IModuleCatalog moduleCatalog) where T : IModule
        {
            var moduleType = typeof(T);
            moduleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = moduleType.FullName,
                                            ModuleType = moduleType.AssemblyQualifiedName,
                                        });

        }
        public static IModuleDefinition Define<T>(this IModuleCatalog moduleCatalog) where T : IModule
        {
            var moduleType = typeof(T);
            var moduleInfo = new ModuleInfo
                {
                    ModuleName = moduleType.FullName,
                    ModuleType = moduleType.AssemblyQualifiedName,
                };
            return new ModuleDefinition(moduleCatalog, moduleInfo);
        }

        private sealed class ModuleDefinition : IModuleDefinition
        {
            private readonly IModuleCatalog _catalog;
            private readonly ModuleInfo _moduleInfo;

            public ModuleDefinition(IModuleCatalog catalog, ModuleInfo moduleInfo)
            {
                _catalog = catalog;
                _moduleInfo = moduleInfo;
            }

            public IModuleDefinition DependsOn<T>() where T : IModule
            {
                var dependency = typeof (T);
                _moduleInfo.DependsOn.Add(dependency.FullName);
                return new ModuleDefinition(_catalog, _moduleInfo);
            }

            public void Add()
            {
                _catalog.AddModule(_moduleInfo);
            }
        }
    }

    public interface IModuleDefinition
    {
        IModuleDefinition DependsOn<T>() where T : IModule;
        void Add();
    }
}
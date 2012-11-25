using Microsoft.Practices.Prism.Modularity;

namespace CallWall.PrismExtensions
{
    public static class ModuleCatalogExtensions
    {
        public static void Add<T>(this IModuleCatalog moduleCatalog)
        {
            var moduleType = typeof(T);
            moduleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = moduleType.Name,
                                            ModuleType = moduleType.AssemblyQualifiedName,
                                        });

        }
    }
}
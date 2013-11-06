using CallWall.Contract;
using Microsoft.Practices.Unity;

namespace CallWall
{
    class TypeRegistry : ITypeRegistry
    {
        private readonly IUnityContainer _container;

        public TypeRegistry(IUnityContainer container)
        {
            _container = container;
        }

        public void RegisterTypeAsSingleton<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>(new ContainerControlledLifetimeManager());
        }

        public void RegisterTypeAsTransient<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>(new TransientLifetimeManager());
        }

        public void RegisterCompositeAsSingleton<TFrom, TTo>() where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(typeof(TTo).FullName, new ContainerControlledLifetimeManager());
        }

        public void RegisterCompositeAsSingleton<TBase, TSpecialization, TImplementation>()
            where TImplementation : TSpecialization
            where TSpecialization : TBase
        {
            //http://www.stackoverflow.com/questions/10910237

            //I don't chain the calls here just to make mocking a touch easier. -LC
            _container.RegisterType<TSpecialization, TImplementation>(new ContainerControlledLifetimeManager());
            _container.RegisterType<TBase, TImplementation>(
                typeof(TImplementation).Name,
                new ExternallyControlledLifetimeManager(),
                new InjectionFactory(u => u.Resolve<TSpecialization>()));
        }
    }
}
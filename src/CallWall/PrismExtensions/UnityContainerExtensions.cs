using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace CallWall.PrismExtensions
{
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Used to register a type as a named registration of a Base/Shared type and also as the default implementation of a specialized interface.
        /// </summary>
        /// <typeparam name="TBase">The base interface to be registered as a named instance to.</typeparam>
        /// <typeparam name="TSpecialization">The specialized interface.</typeparam>
        /// <typeparam name="TImplementation">The actual implementation type</typeparam>
        /// <param name="container">The container to register the types to.</param>
        /// <returns>Returns the container passed in to allow a fluent interface to be maintained.</returns>
        /// <remarks>
        /// This is normally used for registering a type that is to be used in a collection of implementations i.e. is injected 
        /// as part of an <see cref="IEnumerable{TBase}"/>, but that implementation also needs to be referenced directly. You could use
        /// name but then you have nasty strings in your code, and potentially you need to extend the base interface for the specialization.
        /// </remarks>
        public static IUnityContainer RegisterComposite<TBase, TSpecialization, TImplementation>(this IUnityContainer container)
            where TImplementation : TSpecialization
            where TSpecialization : TBase
        {
            //http://www.stackoverflow.com/questions/10910237

            //I don't chain the calls here just to make mocking a touch easier. -LC
            container.RegisterType<TSpecialization, TImplementation>(new ContainerControlledLifetimeManager());
            container.RegisterType<TBase, TImplementation>(
                typeof(TImplementation).Name,
                new ExternallyControlledLifetimeManager(),
                new InjectionFactory(u => u.Resolve<TSpecialization>()));
            return container;
        }
    }
}

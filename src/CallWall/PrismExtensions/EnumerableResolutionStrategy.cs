using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

//Taken from http://pwlodek.blogspot.co.uk/2010/05/lazy-and-ienumerable-support-comes-to.html
namespace CallWall.PrismExtensions
{
    /// <summary>
    /// This strategy implements the logic that will return all instances
    /// when an <see cref="IEnumerable{T}"/> parameter is detected.
    /// </summary>
    public class EnumerableResolutionStrategy : BuilderStrategy
    {
        private delegate object Resolver(IBuilderContext context);

        private static readonly MethodInfo _genericResolveEnumerableMethod =
            typeof(EnumerableResolutionStrategy).GetMethod("ResolveEnumerable", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

        private static readonly MethodInfo _genericResolveLazyEnumerableMethod =
            typeof(EnumerableResolutionStrategy).GetMethod("ResolveLazyEnumerable", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Do the PreBuildUp stage of construction. This is where the actual work is performed.
        /// </summary>
        /// <param name="context">Current build context.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (IsResolvingIEnumerable(context.BuildKey.Type))
            {
                MethodInfo resolverMethod;
                Type typeToBuild = GetTypeToBuild(context.BuildKey.Type);

                if (IsResolvingLazy(typeToBuild))
                {
                    typeToBuild = GetTypeToBuild(typeToBuild);
                    resolverMethod = _genericResolveLazyEnumerableMethod.MakeGenericMethod(typeToBuild);
                }
                else
                {
                    resolverMethod = _genericResolveEnumerableMethod.MakeGenericMethod(typeToBuild);
                }

                var resolver = (Resolver)Delegate.CreateDelegate(typeof(Resolver), resolverMethod);
                context.Existing = resolver(context);
                context.BuildComplete = true;
            }
        }

        private static Type GetTypeToBuild(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        private static bool IsResolvingIEnumerable(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private static bool IsResolvingLazy(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Lazy<>);
        }

        // ReSharper disable UnusedMember.Local
        //Members access via reflection.
        private static object ResolveLazyEnumerable<T>(IBuilderContext context)
        {
            var container = context.NewBuildUp<IUnityContainer>();
            var typeToBuild = typeof(T);
            var typeWrapper = typeof(Lazy<T>);
            var results = ResolveAll(container, typeToBuild, typeWrapper).OfType<Lazy<T>>().ToList();

            return results;
        }

        private static object ResolveEnumerable<T>(IBuilderContext context)
        {
            var container = context.NewBuildUp<IUnityContainer>();
            var typeToBuild = typeof(T);
            var results = ResolveAll(container, typeToBuild, typeToBuild).OfType<T>().ToList();

            return results;
        }
        // ReSharper restore UnusedMember.Local

        private static IEnumerable<object> ResolveAll(IUnityContainer container, Type type, Type typeWrapper)
        {
            var names = GetRegisteredNames(container, type);
            if (type.IsGenericType)
            {
                names = names.Concat(GetRegisteredNames(container, type.GetGenericTypeDefinition()));
            }

            return names.Distinct()
                        .Select(t => t.Name)
                        .Select(name => container.Resolve(typeWrapper, name));
        }

        private static IEnumerable<ContainerRegistration> GetRegisteredNames(IUnityContainer container, Type type)
        {
            return container.Registrations.Where(t => t.RegisteredType == type);
        }
    }
}

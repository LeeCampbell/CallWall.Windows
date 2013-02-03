using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace CallWall.Testing
{
    public sealed class StubUnityContainer : IUnityContainer
    {
        private readonly List<RegisteredType> _registeredTypes = new List<RegisteredType>();
        private readonly List<RegisteredInstance> _registeredInstances = new List<RegisteredInstance>();

        public void Dispose()
        { }

        #region Implementation of IUnityContainer

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            var registration = new RegisteredType { From = @from, To = to, Name = name, LifetimeManager = lifetimeManager, InjectionMembers = injectionMembers };
            RegisteredTypes.Add(registration);
            return this;
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            var registeredInstance = new RegisteredInstance { Type = t, Name = name, Instance = instance, LifetimeManager = lifetime };
            RegisteredInstances.Add(registeredInstance);
            return this;
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return null;
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return new object[] { };
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return null;

        }

        public void Teardown(object o)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer Parent
        {
            get { return null; }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { throw new NotImplementedException(); }
        }



        #endregion

        public List<RegisteredType> RegisteredTypes
        {
            get { return _registeredTypes; }
        }

        public List<RegisteredInstance> RegisteredInstances
        {
            get { return _registeredInstances; }
        }
    }
}
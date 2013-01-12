using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace CallWall.UnitTests.Settings
{
    public sealed class StubUnityContainer : IUnityContainer
    {
        private readonly List<RegisteredType> _registeredTypes = new List<RegisteredType>();
        private readonly List<RegisteredInstance> _registeredInstances = new List<RegisteredInstance>();

        public void Dispose()
        {}

        #region Implementation of IUnityContainer

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            var registration = new RegisteredType { From = @from, To = to, Name = name, LifetimeManager = lifetimeManager };
            RegisteredTypes.Add(registration);
            return this;
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            var registeredInstance = new RegisteredInstance{Type = t, Name = name, Instance = instance, LifetimeManager = lifetime};
            RegisteredInstances.Add(registeredInstance);
            return this;
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return null;
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return new object[] {};
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


    public sealed class RegisteredType
    {
        public Type From { get; set; }

        public Type To { get; set; }

        public string Name { get; set; }

        public LifetimeManager LifetimeManager { get; set; }

        public override string ToString()
        {
            return string.Format("RegisteredType{{From={0}, To={1}, Name={2}, LM={3}}}", From.Name, To.Name, Name, LifetimeManager);
        }
    }

    public sealed class RegisteredInstance
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public object Instance { get; set; }

        public LifetimeManager LifetimeManager { get; set; }

        public override string ToString()
        {
            return string.Format("RegisteredInstance{{Type={0}, Name={1}, Instance={2}, LM={3}}}", Type.Name, Name, Instance, LifetimeManager);
        }
    }
}
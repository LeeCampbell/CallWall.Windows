using System;
using Microsoft.Practices.Unity;

namespace CallWall.Testing
{
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
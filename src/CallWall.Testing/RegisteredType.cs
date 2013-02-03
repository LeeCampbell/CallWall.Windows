using System;
using Microsoft.Practices.Unity;

namespace CallWall.Testing
{
    public sealed class RegisteredType
    {
        public Type From { get; set; }

        public Type To { get; set; }

        public string Name { get; set; }

        public LifetimeManager LifetimeManager { get; set; }

        public InjectionMember[] InjectionMembers { get; set; }

        public override string ToString()
        {
            return string.Format("RegisteredType{{From={0}, To={1}, Name={2}, LM={3}}}", From.Name, To.Name, Name, LifetimeManager);
        }
    }
}
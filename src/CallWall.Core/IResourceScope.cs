using System;

namespace CallWall
{
    public interface IResourceScope
    {
        string Name { get; }
        Uri Resource { get; }
    }
}
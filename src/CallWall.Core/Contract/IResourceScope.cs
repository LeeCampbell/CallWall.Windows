using System;

namespace CallWall.Contract
{
    public interface IResourceScope
    {
        string Name { get; }
        Uri Resource { get; }
    }
}
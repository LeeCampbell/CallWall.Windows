using System;

namespace CallWall.Windows.Contract
{
    public interface IResourceScope
    {
        string Name { get; }
        Uri Resource { get; }
    }
}
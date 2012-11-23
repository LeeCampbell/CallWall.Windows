using System;

namespace CallWall
{
    public interface IProviderDescription
    {
        string Name { get; }
        Uri Image { get; }
    }
}
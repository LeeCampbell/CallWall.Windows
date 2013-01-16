using System;

namespace CallWall.Contract
{
    public interface IProviderDescription
    {
        string Name { get; }
        Uri Image { get; }
    }
}
using System;

namespace CallWall.Windows.Contract
{
    public interface IProviderDescription
    {
        string Name { get; }
        Uri Image { get; }
    }
}
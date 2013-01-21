using System;

namespace CallWall.Contract.Picture
{
    public interface IPicture
    {
        DateTimeOffset Timestamp { get; }
        Uri Source { get; }
        string Caption { get; }
    }
}
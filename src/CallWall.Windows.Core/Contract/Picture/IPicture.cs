using System;

namespace CallWall.Windows.Contract.Picture
{
    public interface IPicture
    {
        DateTimeOffset Timestamp { get; }
        Uri Source { get; }
        string Caption { get; }
    }
}
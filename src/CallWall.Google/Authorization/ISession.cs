using System;

namespace CallWall.Google.Authorization
{
    public interface ISession
    {
        string AccessToken { get; }
        string RefreshToken { get; }
        DateTimeOffset Expires { get; }
        bool HasExpired();
    }
}
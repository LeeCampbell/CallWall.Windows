using System;

namespace CallWall.Google.Authorization
{
    public interface ISessionFactory
    {
        ISession Create(string accessTokenResponse, DateTimeOffset requestedAt);
        ISession Create(string refreshTokenResponse, DateTimeOffset requestedAt, string refreshToken);
    }
}
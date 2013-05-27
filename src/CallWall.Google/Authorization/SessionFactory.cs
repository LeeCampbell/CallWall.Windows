using System;
using Newtonsoft.Json.Linq;

namespace CallWall.Google.Authorization
{
    public sealed class SessionFactory : ISessionFactory
    {
        public ISession Create(string accessTokenResponse, DateTimeOffset requestedAt)
        {
            var json = JObject.Parse(accessTokenResponse);
            return new Session(
                (string) json["access_token"],
                (string) json["refresh_token"],
                TimeSpan.FromSeconds((int) json["expires_in"]),
                requestedAt);
        }

        public ISession Create(string refreshTokenResponse, DateTimeOffset requestedAt, string refreshToken)
        {
            var payload = JObject.Parse(refreshTokenResponse);
            return new Session(
                (string) payload["access_token"],
                refreshToken,
                TimeSpan.FromSeconds((int) payload["expires_in"]),
                requestedAt);
        }
    }
}
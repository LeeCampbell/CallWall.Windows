using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CallWall.Windows.Google.Authorization
{
    public sealed class SessionFactory : ISessionFactory
    {
        public ISession Create(string accessTokenResponse, DateTimeOffset requestedAt, IEnumerable<Uri> requestedResources)
        {
            var json = JObject.Parse(accessTokenResponse);
            return new Session(
                (string) json["access_token"],
                (string) json["refresh_token"],
                TimeSpan.FromSeconds((int) json["expires_in"]),
                requestedAt,
                requestedResources);
        }

        public ISession Create(string refreshTokenResponse, DateTimeOffset requestedAt, string refreshToken, IEnumerable<Uri> requestedResources)
        {
            var payload = JObject.Parse(refreshTokenResponse);
            return new Session(
                (string) payload["access_token"],
                refreshToken,
                TimeSpan.FromSeconds((int) payload["expires_in"]),
                requestedAt,
                requestedResources);
        }
    }
}
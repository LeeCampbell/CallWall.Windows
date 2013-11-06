using System;
using System.Collections.Generic;

namespace CallWall.Windows.Google.Authorization
{
    public interface ISessionFactory
    {
        ISession Create(string accessTokenResponse, DateTimeOffset requestedAt, IEnumerable<Uri> requestedResources);
        ISession Create(string refreshTokenResponse, DateTimeOffset requestedAt, string refreshToken, IEnumerable<Uri> requestedResources);
    }
}
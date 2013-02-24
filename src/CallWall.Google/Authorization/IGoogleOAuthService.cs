using System;
using System.Collections.Generic;

namespace CallWall.Google.Authorization
{
    public interface IGoogleOAuthService
    {
        Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources);
        IObservable<ISession> RequestAccessToken(string authorizationCode);
        IObservable<ISession> RequestRefreshedAccessToken(string refreshToken);
    }
}
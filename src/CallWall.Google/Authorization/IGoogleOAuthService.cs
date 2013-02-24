using System;

namespace CallWall.Google.Authorization
{
    public interface IGoogleOAuthService
    {
        Uri BuildAuthorizationUri(Uri[] requestedResources);
        IObservable<ISession> RequestAccessToken(string authorizationCode);
        IObservable<ISession> RequestRefreshedAccessToken(string refreshToken);
    }
}
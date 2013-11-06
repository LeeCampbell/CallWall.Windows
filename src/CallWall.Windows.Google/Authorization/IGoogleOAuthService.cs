using System;
using System.Collections.Generic;

namespace CallWall.Windows.Google.Authorization
{
    public interface IGoogleOAuthService
    {
        Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources);
        IObservable<ISession> RequestAccessToken(string authorizationCode, IEnumerable<Uri> requestedResources);
        IObservable<ISession> RequestRefreshedAccessToken(string refreshToken, IEnumerable<Uri> authorizedResources);
    }
}
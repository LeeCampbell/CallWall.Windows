using System;
using System.Collections.Generic;
using CallWall.Windows.Web;

namespace CallWall.Windows.Google.Authorization
{
    public interface IOAuthUriFactory
    {
        Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources);
        HttpRequestParameters CreateAccessTokenWebRequest(string authorizationCode);
        HttpRequestParameters CreateRefreshTokenWebRequest(string refreshToken);
    }
}
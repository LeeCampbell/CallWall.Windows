using System;
using System.Collections.Generic;
using System.Linq;
using CallWall.Web;

namespace CallWall.Google.Authorization
{
    public sealed class OAuthUriFactory : IOAuthUriFactory
    {
        //HACK:Potentially this should be retrieved from config so it can not be decompiled. -LC
        private const string ClientId = "410654176090.apps.googleusercontent.com";  //} TODO:This is all the Spike stuff. Might need to change.
        //HACK:Potentially this should be retrieved from config so it can not be decompiled. -LC
        private const string ClientSecret = "bDkwW8Y2RnUt0JsjbAwYA8cb";             //} I probably will eventually change this to be related to a CallWall.com email/google address.
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        public Uri BuildAuthorizationUri(IEnumerable<Uri> requestedResources)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["response_type"] = "code";
            queryString["client_id"] = ClientId;                                    //Lee.Ryan.Campbell@gmail.com client Id
            queryString["redirect_uri"] = RedirectUri;
            var scopes = requestedResources.Select(uri => uri.ToString()).ToArray();
            queryString["scope"] = string.Join(" ", scopes);                        //" " should be translated into a "+" as per https://developers.google.com/accounts/docs/OAuth2InstalledApp

            var authorizationUri = new UriBuilder(@"https://accounts.google.com/o/oauth2/auth");
            authorizationUri.Query = queryString.ToString();                        // Returns "key1=value1&key2=value2", all URL-encoded
            return authorizationUri.Uri;
        }

        public HttpRequestParameters CreateAccessTokenWebRequest(string authorizationCode)
        {
            var requestParams = new HttpRequestParameters(@"https://accounts.google.com/o/oauth2/token");

            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("redirect_uri", RedirectUri);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("grant_type", "authorization_code");
            requestParams.PostParameters.Add("code", authorizationCode);

            return requestParams;
        }

        public HttpRequestParameters CreateRefreshTokenWebRequest(string refreshToken)
        {
            var requestParams = new HttpRequestParameters(@"https://accounts.google.com/o/oauth2/token");
            requestParams.PostParameters.Add("client_id", ClientId);
            requestParams.PostParameters.Add("client_secret", ClientSecret);
            requestParams.PostParameters.Add("grant_type", "refresh_token");
            requestParams.PostParameters.Add("refresh_token", refreshToken);

            return requestParams;
        }
    }
}
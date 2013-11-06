using System.Linq;
using CallWall.Windows.Google.AccountConfiguration;
using CallWall.Windows.Google.Authorization;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Google.UnitTests.Authorization
{
    [TestFixture]
    public class OAuthUriFactoryFixture
    {
        private OAuthUriFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new OAuthUriFactory();
        }

        [Test]
        public void BuildAuthorizationUri_should_set_base_UriPath()
        {
            var authUri = _factory.BuildAuthorizationUri(new[] { GoogleResource.Gmail.Resource });

            //https://accounts.google.com/o/oauth2/auth
            Assert.AreEqual(@"https", authUri.Scheme);
            Assert.AreEqual(@"accounts.google.com", authUri.Host);
            Assert.AreEqual(@"/o/oauth2/auth", authUri.LocalPath);
        }
        [Test]
        public void BuildAuthorizationUri_should_set_response_type()
        {
            var authUri = _factory.BuildAuthorizationUri(new[] { GoogleResource.Gmail.Resource });
            var queryParameters = System.Web.HttpUtility.ParseQueryString(authUri.Query);
            Assert.AreEqual("code", queryParameters["response_type"]);
        }
        [Test]//HACK:Potentially this should be retrieved from server-side configuration so it can not be decompiled. -LC
        public void BuildAuthorizationUri_should_set_client_id()
        {
            var authUri = _factory.BuildAuthorizationUri(new[] { GoogleResource.Gmail.Resource });
            var queryParameters = System.Web.HttpUtility.ParseQueryString(authUri.Query);
            Assert.IsNotNullOrEmpty(queryParameters["client_id"]);
        }
        [Test]
        public void BuildAuthorizationUri_should_set_redirect_uri()
        {
            var authUri = _factory.BuildAuthorizationUri(new[] { GoogleResource.Gmail.Resource });
            var queryParameters = System.Web.HttpUtility.ParseQueryString(authUri.Query);
            Assert.AreEqual("urn:ietf:wg:oauth:2.0:oob", queryParameters["redirect_uri"]);
        }
        [Test]
        public void BuildAuthorizationUri_should_set_scope()
        {
            var resource = GoogleResource.Gmail.Resource;
            var authUri = _factory.BuildAuthorizationUri(new[] { resource });
            var queryParameters = System.Web.HttpUtility.ParseQueryString(authUri.Query);
            Assert.AreEqual(resource.ToString(), queryParameters["scope"]);
        }
        [Test]
        public void BuildAuthorizationUri_should_set_scope_for_multiple_resources()
        {
            var resources = new[] { GoogleResource.Gmail.Resource, GoogleResource.Contacts.Resource };
            var expected = string.Join(" ", resources.Select(uri => uri.ToString()).ToArray());
            var authUri = _factory.BuildAuthorizationUri(resources);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(authUri.Query);
            Assert.AreEqual(expected, queryParameters["scope"]);
        }

        [Test]
        public void CreateAccessTokenWebRequest_should_set_Uri()
        {
            var request = _factory.CreateAccessTokenWebRequest("token");
            Assert.AreEqual(@"https://accounts.google.com/o/oauth2/token", request.EndPointUrl);
        }
        [Test]//HACK:Potentially this should be retrieved from server-side configuration so it can not be decompiled. -LC
        public void CreateAccessTokenWebRequest_should_set_client_id()
        {
            var request = _factory.CreateAccessTokenWebRequest("token");
            Assert.IsNotNullOrEmpty(request.PostParameters["client_id"]);
        }
        [Test]//HACK:Potentially this should be retrieved from server-side configuration so it can not be decompiled. -LC
        public void CreateAccessTokenWebRequest_should_set_client_secret()
        {
            var request = _factory.CreateAccessTokenWebRequest("token");
            Assert.IsNotNullOrEmpty(request.PostParameters["client_secret"]);
        }
        [Test]
        public void CreateAccessTokenWebRequest_should_set_grant_type()
        {
            var request = _factory.CreateAccessTokenWebRequest("token");
            Assert.AreEqual("authorization_code", request.PostParameters["grant_type"]);
        }
        [Test]
        public void CreateAccessTokenWebRequest_should_set_redirect_uri()
        {
            var request = _factory.CreateAccessTokenWebRequest("token");
            Assert.AreEqual("urn:ietf:wg:oauth:2.0:oob", request.PostParameters["redirect_uri"]);
        }
        [Test]
        public void CreateAccessTokenWebRequest_should_set_code()
        {
            var authorizationCode = "token";
            var request = _factory.CreateAccessTokenWebRequest(authorizationCode);
            Assert.AreEqual(authorizationCode, request.PostParameters["code"]);
        }

        [Test]
        public void CreateRefreshTokenWebRequest_should_set_Uri()
        {
            var request = _factory.CreateRefreshTokenWebRequest("token");
            Assert.AreEqual(@"https://accounts.google.com/o/oauth2/token", request.EndPointUrl);
        }
        [Test]//HACK:Potentially this should be retrieved from server-side configuration so it can not be decompiled. -LC
        public void CreateRefreshTokenWebRequest_should_set_client_id()
        {
            var request = _factory.CreateRefreshTokenWebRequest("token");
            Assert.IsNotNullOrEmpty(request.PostParameters["client_id"]);
        }
        [Test]//HACK:Potentially this should be retrieved from server-side configuration so it can not be decompiled. -LC
        public void CreateRefreshTokenWebRequest_should_set_client_secret()
        {
            var request = _factory.CreateRefreshTokenWebRequest("token");
            Assert.IsNotNullOrEmpty(request.PostParameters["client_secret"]);
        }
        [Test]
        public void CreateRefreshTokenWebRequest_should_set_grant_type()
        {
            var request = _factory.CreateRefreshTokenWebRequest("token");
            Assert.AreEqual("refresh_token", request.PostParameters["grant_type"]);
        }
        [Test]
        public void CreateRefreshTokenWebRequest_should_refresh_token()
        {
            var refreshToken = "token";
            var request = _factory.CreateRefreshTokenWebRequest(refreshToken);
            Assert.AreEqual(refreshToken, request.PostParameters["refresh_token"]);
        }
    }
}
// ReSharper restore InconsistentNaming
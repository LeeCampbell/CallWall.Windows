using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CallWall.Google.AccountConfiguration;
using CallWall.Google.Authorization;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.Authorization
{
    [TestFixture]
    public class SessionFactoryFixture
    {
        private SessionFactory _factory;
        private string _accessTokenResponse;
        private string _refreshTokenResponse;
        private string _expectedAccessToken;
        private string _expectedRefreshToken;
        private int _expectedExpiresIn;
        private ISet<Uri> _expectedAuthorizedUris;

        [SetUp]
        public void SetUp()
        {
            _factory = new SessionFactory();

            _expectedAccessToken = "expectedAccessToken";
            _expectedRefreshToken = "expectedRefreshToken";
            _expectedExpiresIn = 3600;
            _expectedAuthorizedUris = new HashSet<Uri>(GoogleResource.AvailableResourceScopes.Select(r => r.Resource));
            _accessTokenResponse = "{\"access_token\" : \"expectedAccessToken\",\"token_type\" : \"Bearer\", \"expires_in\" : 3600, \"refresh_token\" : \"expectedRefreshToken\"}";
            _refreshTokenResponse = "{\"access_token\" : \"expectedAccessToken\", \"token_type\" : \"Bearer\", \"expires_in\" : 3600 }";
        }
        [Test]
        public void Create_from_accessTokenResponse_Should_set_AccessToken_from_response()
        {
            var actual = _factory.Create(_accessTokenResponse, DateTimeOffset.Now, _expectedAuthorizedUris);
            Assert.AreEqual(_expectedAccessToken, actual.AccessToken);
        }
        [Test]
        public void Create_from_accessTokenResponse_Should_set_RefreshToken_from_response()
        {
            var actual = _factory.Create(_accessTokenResponse, DateTimeOffset.Now, _expectedAuthorizedUris);
            Assert.AreEqual(_expectedRefreshToken, actual.RefreshToken);
        }
        [Test]
        public void Create_from_accessTokenResponse_Should_set_Expires_from_response()
        {
            var now = DateTimeOffset.Now;
            var actual = _factory.Create(_accessTokenResponse, now, _expectedAuthorizedUris);
            Assert.AreEqual(now.AddSeconds(_expectedExpiresIn), actual.Expires);
        }
        [Test]
        public void Create_from_accessTokenResponse_Should_set_HasExpired_to_false()
        {
            var actual = _factory.Create(_accessTokenResponse, DateTimeOffset.Now, _expectedAuthorizedUris);
            Assert.IsFalse(actual.HasExpired());
        }


        [Test]
        public void Create_from_refreshTokenResponse_Should_set_AccessToken_from_response()
        {
            var actual = _factory.Create(_refreshTokenResponse, DateTimeOffset.Now, _expectedRefreshToken, _expectedAuthorizedUris);
            Assert.AreEqual(_expectedAccessToken, actual.AccessToken);
        }
        [Test]
        public void Create_from_refreshTokenResponse_Should_set_RefreshToken_from_response()
        {
            var actual = _factory.Create(_refreshTokenResponse, DateTimeOffset.Now, _expectedRefreshToken, _expectedAuthorizedUris);
            Assert.AreEqual(_expectedRefreshToken, actual.RefreshToken);
        }
        [Test]
        public void Create_from_refreshTokenResponse_Should_set_Expires_from_response()
        {
            var now = DateTimeOffset.Now;
            var actual = _factory.Create(_refreshTokenResponse, now, _expectedRefreshToken, _expectedAuthorizedUris);
            Assert.AreEqual(now.AddSeconds(_expectedExpiresIn), actual.Expires);
        }
        [Test]
        public void Create_from_refreshTokenResponse_Should_set_HasExpired_to_false()
        {
            var actual = _factory.Create(_refreshTokenResponse, DateTimeOffset.Now, _expectedRefreshToken, _expectedAuthorizedUris);
            Assert.IsFalse(actual.HasExpired());
        }

        [Test]
        public void Session_ToString_specifies_each_property()
        {
            var now = DateTimeOffset.Now;

            var expected = new StringBuilder();
            expected.Append("Session { ");
            expected.AppendFormat("AccessToken : '{0}', ", _expectedAccessToken);
            expected.AppendFormat("RefreshToken : '{0}', ", _expectedRefreshToken);
            expected.AppendFormat("Expires : '{0:o}'", now.AddSeconds(_expectedExpiresIn));
            expected.Append("}");

            var actual = _factory.Create(_refreshTokenResponse, now, _expectedRefreshToken, _expectedAuthorizedUris);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
// ReSharper restore InconsistentNaming
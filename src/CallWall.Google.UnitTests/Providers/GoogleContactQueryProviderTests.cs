using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.Google.Authorization;
using CallWall.Google.Providers;
using CallWall.Web;
using Moq;
using NUnit.Framework;

namespace CallWall.Google.UnitTests.Providers
{
    public abstract class Given_a_constructed_GoogleContactQueryProvider
    {
        private GoogleContactQueryProvider _sut;
        private Mock<IGoogleAuthorization> _authorizationMock;
        private StubHttpClient _httpClient;
        private Mock<IGoogleContactProfileTranslator> _translatorMock;
        private Mock<ILogger> _loggerStub;

        [SetUp]
        public virtual void Setup()
        {
            _authorizationMock = new Mock<IGoogleAuthorization>();
            _httpClient = new StubHttpClient();
            _translatorMock = new Mock<IGoogleContactProfileTranslator>();
            var logFactoryStub = new Mock<ILoggerFactory>();
            _loggerStub = new Mock<ILogger>();
            logFactoryStub.Setup(lf => lf.CreateLogger()).Returns(_loggerStub.Object);
            _sut = new GoogleContactQueryProvider(_authorizationMock.Object, _httpClient, _translatorMock.Object, logFactoryStub.Object);
        }

        [TestFixture]
        public sealed class When_loading_ContactProfile : Given_a_constructed_GoogleContactQueryProvider
        {
            //Should Create a request from the Authorization code, pass to WebClient & return translated response.

            #region Setup

            private IProfile _profile;
            private string _accessToken = "The access token";
            private string _webResponse = "The web response";
            private Mock<IContactProfile> _contactProfileMock;

            public override void Setup()
            {
                base.Setup();
                _authorizationMock.Setup(a => a.RequestAccessToken())
                    .Returns(Observable.Return(_accessToken));
                _contactProfileMock = new Mock<IContactProfile>();
                _translatorMock.Setup(t => t.Translate(_webResponse, _accessToken))
                    .Returns(_contactProfileMock.Object);
                _profile = CreateProfile();
                _httpClient.Response = _webResponse;
            }

            private static IProfile CreateProfile()
            {
                var inputStub = new Mock<IProfile>();
                var identifier = new Mock<IPersonalIdentifier>();
                identifier.Setup(i => i.IdentifierType).Returns("Phone");
                identifier.Setup(i => i.Value).Returns("+64 25 1234 5678");
                //identifier.Setup(i => i.Provider).Returns(null);
                var identifiers = new[] {identifier.Object};
                inputStub.Setup(i => i.Identifiers).Returns(identifiers);
                return inputStub.Object;
            }

            #endregion

            [Test]
            public void Should_call_webClient_with_GoogleContactURI()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(@"https://www.google.com/m8/feeds/contacts/default/full", _httpClient.LastRequest.EndPointUrl);
            }
            [Test]
            public void Should_call_webClient_with_AccessToken()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_accessToken, _httpClient.LastRequest.QueryStringParameters["access_token"]);
            }
            [Test]
            public void Should_call_webClient_with_Identifier_value()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_profile.Identifiers.Single().Value, _httpClient.LastRequest.QueryStringParameters["q"]);
            }
            [Test]
            public void Should_call_webClient_request_version_3_of_the_contract()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual("3.0", _httpClient.LastRequest.Headers["GData-Version"]);
            }

            [Test]
            public void Should_call_translator_with_web_response()
            {
                _sut.LoadContact(_profile).Subscribe();

                _translatorMock.Verify();//Redundant as the final response only works if this is met
            }

            [Test]
            public void Should_return_translator_result()
            {
                var actual = _sut.LoadContact(_profile).First();

                Assert.AreSame(_contactProfileMock.Object, actual);
            }
        }

        private sealed class StubHttpClient : IHttpClient
        {
            public string Response { get; set; }
            public HttpRequestParameters LastRequest { get; private set; }

            public IObservable<string> GetResponse(HttpRequestParameters request)
            {
                LastRequest = request;
                return Observable.Return(Response);
            }
        }
    }
}
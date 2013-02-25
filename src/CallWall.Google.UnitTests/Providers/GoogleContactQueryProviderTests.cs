using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CallWall.Contract;
using CallWall.Google.AccountConfiguration;
using CallWall.Google.Authorization;
using CallWall.Google.Providers;
using CallWall.Google.Providers.Contacts;
using CallWall.Web;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.Providers
{
    public abstract class Given_a_constructed_GoogleContactQueryProvider
    {
        private GoogleContactQueryProvider _sut;
        private Mock<IGoogleAuthorization> _authorizationMock;
        private StubHttpClient _httpClient;
        private Mock<IGoogleContactProfileTranslator> _translatorMock;
        private Mock<ILogger> _loggerStub;
        private Mock<ILoggerFactory> _logFactoryStub;

        [SetUp]
        public virtual void Setup()
        {
            _authorizationMock = new Mock<IGoogleAuthorization>();
            _translatorMock = new Mock<IGoogleContactProfileTranslator>();
            _logFactoryStub = new Mock<ILoggerFactory>();
            _loggerStub = new Mock<ILogger>();
            _logFactoryStub.Setup(lf => lf.CreateLogger(It.IsAny<Type>())).Returns(_loggerStub.Object);
            
        }

        [TestFixture]
        public sealed class When_loading_ContactProfile : Given_a_constructed_GoogleContactQueryProvider
        {
            //Should Create a request from the Authorization code, 
            //  -->pass to WebClient 
            //  -->Translate
            //  -->request groups from webclient
            //  -->pass groups response and tranlated contact to translator for enrichment of groups
            //  -->return the enriched contact.

            #region Setup
            private const string _contactsUri = @"https://www.google.com/m8/feeds/contacts/default/full";
            private const string _groupsUri = @"https://www.google.com/m8/feeds/groups/default/full";

            private IProfile _profile;
            private string _accessToken = "The access token";
            private string _contactWebResponse = "The contact response";
            private string _groupsWebResponse = "The groups response";
            private Mock<IGoogleContactProfile> _contactProfileMock;
            private Mock<IGoogleContactProfile> _enrichedProfileMock;

            public override void Setup()
            {
                base.Setup();
                _authorizationMock.SetupGet(a => a.Status).Returns(Google.Authorization.AuthorizationStatus.Authorized);
                _authorizationMock.Setup(a => a.RequestAccessToken(GoogleResource.Contacts))
                    .Returns(Observable.Return(_accessToken));
                _contactProfileMock = new Mock<IGoogleContactProfile>();
                _enrichedProfileMock = new Mock<IGoogleContactProfile>();
                _translatorMock.Setup(t => t.Translate(_contactWebResponse, _accessToken))
                    .Returns(_contactProfileMock.Object);
                _translatorMock.Setup(t => t.AddTags(_contactProfileMock.Object, _groupsWebResponse))
                    .Returns(_enrichedProfileMock.Object);
                _profile = CreateProfile();

                _httpClient = new StubHttpClient(request=>
                                                     {
                                                         if(request.EndPointUrl == _contactsUri)
                                                         {
                                                             return _contactWebResponse;
                                                         }
                                                         if(request.EndPointUrl == _groupsUri)
                                                         {
                                                             return _groupsWebResponse;
                                                         }
                                                         return null;
                                                     });
                _sut = new GoogleContactQueryProvider(_authorizationMock.Object, _httpClient, _translatorMock.Object, _logFactoryStub.Object);
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
            public void Should_call_webClient_first_with_GoogleContactURI()
            {
                _sut.LoadContact(_profile).Subscribe();
                
                Assert.AreEqual(_contactsUri, _httpClient.Requests[0].EndPointUrl);
            }
            [Test]
            public void Should_call_webClient_first_with_AccessToken()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_accessToken, _httpClient.Requests[0].QueryStringParameters["access_token"]);
            }
            [Test]
            public void Should_call_webClient_first_with_Identifier_value()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_profile.Identifiers.Single().Value, _httpClient.Requests[0].QueryStringParameters["q"]);
            }
            [Test]
            public void Should_call_webClient_first_with_version_3_of_the_contract()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual("3.0", _httpClient.Requests[0].Headers["GData-Version"]);
            }

            [Test]
            public void Should_call_translator_with_web_response()
            {
                _sut.LoadContact(_profile).Subscribe();

                _translatorMock.Verify();//Redundant as the final response only works if this is met
            }

            [Test]
            public void Should_call_webClient_second_with_GoogleContactGroupUri()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_groupsUri, _httpClient.Requests[1].EndPointUrl);
            }
            [Test]
            public void Should_call_webClient_second_with_AccessToken()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual(_accessToken, _httpClient.Requests[1].QueryStringParameters["access_token"]);
            }
            [Test]
            public void Should_call_webClient_second_with_version_3_of_the_contract()
            {
                _sut.LoadContact(_profile).Subscribe();

                Assert.AreEqual("3.0", _httpClient.Requests[1].Headers["GData-Version"]);
            }

            [Test]
            public void Should_return_translator_result()
            {
                var actual = _sut.LoadContact(_profile).First();

                Assert.AreSame(_enrichedProfileMock.Object, actual);
            }
        }

        private sealed class StubHttpClient : IHttpClient
        {
            private readonly Func<HttpRequestParameters, string> _responder;

            public StubHttpClient(Func<HttpRequestParameters, string> responder)
            {
                _responder = responder;
                Requests = new List<HttpRequestParameters>();
            }

            public List<HttpRequestParameters> Requests { get; private set; }

            public IObservable<string> GetResponse(HttpRequestParameters request)
            {
                Requests.Add(request);
                return Observable.Return(_responder(request));
            }
        }
    }
}
// ReSharper restore InconsistentNaming
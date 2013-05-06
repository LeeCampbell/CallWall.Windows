using System.Collections.Specialized;
using System.Net;
using System.Web.Http;
using CallWall.Testing;
using CallWall.Web;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.IntegrationTests.Web
{
    public abstract class Given_a_HttpRequestParameters_instance
    {
        private HttpRequestParameters _sut;
        private string _baseAddress = "http://localhost:8080/";
        private string _endpoint = "http://localhost:8080/api/test";
        private InProcessWebServer _server;

        private Given_a_HttpRequestParameters_instance()
        { }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = new InProcessWebServer();
            _server.Start(_baseAddress);
        }
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }



        [SetUp]
        public virtual void Setup()
        {
            _sut = new HttpRequestParameters(_endpoint);
        }

        [TestFixture]
        public sealed class When_creating_a_WebRequest_with_Post_parameters : Given_a_HttpRequestParameters_instance
        {
            private NameValueCollection _expectedPostParams;
            private WebRequest _webRequest;

            public override void Setup()
            {
                base.Setup();

                _expectedPostParams = new NameValueCollection { { "code", "ABCDEF" }, { "client_id", "bob123" } };
                foreach (var key in _expectedPostParams.AllKeys)
                {
                    _sut.PostParameters.Add(key, _expectedPostParams[key]);
                }
            }

            [Test]
            public void Should_return_webrequest_with_Method_as_post()
            {
                _webRequest = _sut.CreateRequest(NullLogger.Instance);
                Assert.AreEqual("POST", _webRequest.Method);
            }

            [Test]
            public void Should_return_webrequest_with_ContentType_set()
            {
                _webRequest = _sut.CreateRequest(NullLogger.Instance);
                Assert.AreEqual("application/x-www-form-urlencoded", _webRequest.ContentType);
            }

            [Test, Ignore("Need to implement a callback loop/host a site to validate with")]
            public void Should_return_webrequest_with_post_params_written_to_requestStream()
            {
                //var expected = PostParamsAsString(_expectedPostParams);
                //var stream = _webRequest.GetRequestStream();
                //var reader = new StreamReader(stream);    //Fail, stream CanRead=false;
                //var actual = reader.ReadToEnd();
                //Assert.AreEqual(expected, actual);


                //using (var handler = new MyHandler())
                //using (var server = new InProcessWebServer())
                //{
                //    server.Start(_baseAddress, handler);
                //    _webRequest = _sut.CreateRequest(NullLogger.Instance);
                //    var response = _webRequest.GetResponseAsync().Wait(TimeSpan.FromSeconds(5));

                //    Console.WriteLine(response);
                //}
            }

            private static string PostParamsAsString(NameValueCollection kvps)
            {
                var result = System.Web.HttpUtility.ParseQueryString(string.Empty);
                foreach (var key in kvps.AllKeys)
                {
                    result[key] = kvps[key];
                }
                return result.ToString();
            }
        }
    }

    public class TestController : ApiController
    {
        public int Test()
        {
            return 1;
        }
    }
}
// ReSharper restore InconsistentNaming
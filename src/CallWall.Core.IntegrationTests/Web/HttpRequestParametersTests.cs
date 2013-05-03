using System.Collections.Specialized;
using System.IO;
using System.Net;
using CallWall.Testing;
using CallWall.Web;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.IntegrationTests.Web
{
    public abstract class Given_a_HttpRequestParameters_instance
    {
        private HttpRequestParameters _sut;
        private string _endpoint = "http://localhost:????";//"http://www.callwall.com";

        private Given_a_HttpRequestParameters_instance()
        { }

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
                _webRequest = _sut.CreateRequest(NullLogger.Instance);
            }

            [Test, Ignore("Need to implement a callback loop/host a site to validate with")]
            public void Should_return_webrequest_with_Method_as_post()
            {
                Assert.AreEqual("POST", _webRequest.Method);
            }

            [Test, Ignore("Need to implement a callback loop/host a site to validate with")]
            public void Should_return_webrequest_with_ContentType_set()
            {
                Assert.AreEqual("application/x-www-form-urlencoded", _webRequest.ContentType);
            }

            [Test, Ignore("Need to implement a callback loop/host a site to validate with")]
            public void Should_return_webrequest_with_post_params_written_to_requestStream()
            {
                var expected = PostParamsAsString(_expectedPostParams);
                var stream = _webRequest.GetRequestStream();
                var reader = new StreamReader(stream);
                var actual = reader.ReadToEnd();

                Assert.AreEqual(expected, actual);
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
}
// ReSharper restore InconsistentNaming
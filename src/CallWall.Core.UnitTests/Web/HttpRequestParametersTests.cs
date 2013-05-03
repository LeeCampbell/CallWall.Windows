using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CallWall.Web;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests.Web
{
    public abstract class Given_a_HttpRequestParameters_instance
    {
        private HttpRequestParameters _sut;
        private string _endpoint = "http://www.callwall.com";

        private Given_a_HttpRequestParameters_instance()
        { }

        [SetUp]
        public virtual void Setup()
        {
            _sut = new HttpRequestParameters(_endpoint);
        }

        [Test]
        public void Should_return_provided_endpoint_from_Endpoint_property()
        {
            Assert.AreEqual(_endpoint, _sut.EndPointUrl);
        }

        [TestFixture]
        public sealed class When_constructing_Uri : Given_a_HttpRequestParameters_instance
        {
            [Test]
            public void Should_return_Uri_with_endpoint_as_base()
            {
                var expected = new Uri(_endpoint);
                var uri = _sut.ConstructUri();
                Assert.AreEqual(expected.AbsoluteUri, uri.AbsoluteUri);
            }

            [TestCase("q", "CallWall", "?q=CallWall")]
            [TestCase("q", "CallWall client", "?q=CallWall+client")]
            [TestCase("q", "CallWall & android", "?q=CallWall+%26+android")]
            public void Should_return_QueryString_with_provided_keyValue_pairs_escaped(string key, string value, string expected)
            {
                _sut.QueryStringParameters.Add(key, value);
                var uri = _sut.ConstructUri();
                Assert.AreEqual(expected, uri.Query);
            }

        }

        //NOTE: Adding post params needs to be tested via an integration test.
        [TestFixture]
        public sealed class When_creating_a_request : Given_a_HttpRequestParameters_instance
        {
            [Test]
            public void Should_return_Uri_with_endpoint_as_base()
            {
                var expected = new Uri(_endpoint);
                var uri = _sut.ConstructUri();
                Assert.AreEqual(expected.AbsoluteUri, uri.AbsoluteUri);
            }

            [Test]
            public void Should_add_headers_provided()
            {
                var expected = new WebHeaderCollection();
                _sut.Headers.Add("GData-Version", "3.0");
                expected.Add("GData-Version", "3.0");

                var webRequest = _sut.CreateRequest(new Mock<ILogger>().Object);

                Assert.AreEqual(expected.Count, webRequest.Headers.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    var key = expected.Keys[i];
                    Assert.AreEqual(key, webRequest.Headers.Keys[i]);
                    Assert.AreEqual(expected[key], webRequest.Headers[key]);
                }
            }
        }
    }
}
// ReSharper restore InconsistentNaming
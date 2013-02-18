using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CallWall.Google.Authorization;
using CallWall.Web;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.Authorization
{
    public abstract class Given_a_newly_constructed_GoogleAuthorization
    {
        private GoogleAuthorization _sut;
        private Mock<IPersonalizationSettings> _localStoreMock;
        private Mock<IHttpClient> _httpClientMock;
        private Mock<ILogger> _loggerMock;

        private Given_a_newly_constructed_GoogleAuthorization()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _localStoreMock = new Mock<IPersonalizationSettings>();
            _httpClientMock = new Mock<IHttpClient>();
            _loggerMock = new Mock<ILogger>();
            var logFactory = new Mock<ILoggerFactory>();
            logFactory.Setup(lf => lf.CreateLogger(It.IsAny<Type>())).Returns(_loggerMock.Object);
            _sut = new GoogleAuthorization(_localStoreMock.Object, _httpClientMock.Object, logFactory.Object);
        }

        [TestFixture]
        public sealed class When_requesting_AccessToken : Given_a_newly_constructed_GoogleAuthorization
        {
            [Test]
            public void Should_OnError_if_no_callback_has_been_registered()
            {
                var observer = new TestScheduler().CreateObserver<string>();
                _sut.AvailableResourceScopes[0].IsEnabled = true;
                _sut.RequestAccessToken().Subscribe(observer);
                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(NotificationKind.OnError, observer.Messages[0].Value.Kind);
                Assert.IsInstanceOf<InvalidOperationException>(observer.Messages[0].Value.Exception);
                Assert.AreEqual("No call-back has been registered via the RegisterAuthorizationCallback method", observer.Messages[0].Value.Exception.Message);
            }

            [Test]
            public void Should_OnError_if_no_Resources_have_been_selected()
            {
                var observer = new TestScheduler().CreateObserver<string>();
                RequestAuthorizationCode callback = uri => Observable.Empty<string>();
                _sut.RegisterAuthorizationCallback(callback);

                _sut.RequestAccessToken().Subscribe(observer);

                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(NotificationKind.OnError, observer.Messages[0].Value.Kind);
                Assert.IsInstanceOf<InvalidOperationException>(observer.Messages[0].Value.Exception);
                Assert.AreEqual("No resources have been enabled.",observer.Messages[0].Value.Exception.Message);
            }

            [Test]
            public void Should_call_the_registered_callback()
            {
                var wasCalledAndSubscribedTo = false;
                RequestAuthorizationCode callback = uri => Observable.Create<string>(
                    o =>
                    {
                        wasCalledAndSubscribedTo = true;
                        return Disposable.Empty;
                    });
                _sut.RegisterAuthorizationCallback(callback);
                _sut.AvailableResourceScopes[0].IsEnabled = true;
                _sut.RequestAccessToken().Subscribe();

                Assert.IsTrue(wasCalledAndSubscribedTo);
            }

            //Should validate that there are some resources enabled.
            //when multiple concurrent requests for access token are made should only call call-back once
            //
        }

        //When calling Login/Authorize callback
            //Should ?? on empty response
            //Should ?? on error response


    }

    //Given an Authorized_GoogleAuthoization
    //  When requesting access token
    //  when requesting access token for modified resources
    //  when multiple concurrent requests for access token are made
    //Given a lapsed GoogleAuthoization

}
// ReSharper restore InconsistentNaming
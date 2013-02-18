using CallWall.Google.Authorization;
using CallWall.Web;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Reactive;
using System.Reactive.Linq;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.Authorization
{
    public abstract class Given_a_newly_constructed_GoogleAuthorization
    {
        #region Setup guff

        private GoogleAuthorization _sut;
        private Mock<IPersonalizationSettings> _localStoreMock;
        private Mock<IHttpClient> _httpClientMock;
        private Mock<ILogger> _loggerMock;

        private Given_a_newly_constructed_GoogleAuthorization()
        {
        }

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

        #endregion

        [TestFixture]
        public sealed class Given_no_registered_GoogleAuthorization_callback : Given_a_newly_constructed_GoogleAuthorization
        {
            [Test]
            public void Should_OnError_RequestForAccess()
            {
                var observer = new TestScheduler().CreateObserver<string>();
                _sut.AvailableResourceScopes[0].IsEnabled = true;
                _sut.RequestAccessToken().Subscribe(observer);
                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(NotificationKind.OnError, observer.Messages[0].Value.Kind);
                Assert.IsInstanceOf<InvalidOperationException>(observer.Messages[0].Value.Exception);
                Assert.AreEqual("No call-back has been registered via the RegisterAuthorizationCallback method",
                                observer.Messages[0].Value.Exception.Message);
            }
        }

        public abstract class Given_a_registered_GoogleAuthorization : Given_a_newly_constructed_GoogleAuthorization
        {
            #region Setup guff

            private Given_a_registered_GoogleAuthorization()
            {
            }

            protected abstract RequestAuthorizationCode CreateCallback();

            public override void SetUp()
            {
                base.SetUp();
                var callback = CreateCallback();
                _sut.RegisterAuthorizationCallback(callback);
                _sut.AvailableResourceScopes[0].IsEnabled = true;   //Set at least one Resource to enabled to allow nominal tests to pass.
            }

            #endregion

            [Test]
            public void Should_OnError_if_no_Resources_have_been_selected()
            {
                var observer = new TestScheduler().CreateObserver<string>();
                RequestAuthorizationCode callback = uri => Observable.Empty<string>();
                _sut.RegisterAuthorizationCallback(callback);
                foreach (var resourceScope in _sut.AvailableResourceScopes)
                {
                    resourceScope.IsEnabled = false;
                }
                _sut.RequestAccessToken().Subscribe(observer);

                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(NotificationKind.OnError, observer.Messages[0].Value.Kind);
                Assert.IsInstanceOf<InvalidOperationException>(observer.Messages[0].Value.Exception);
                Assert.AreEqual("No resources have been enabled.", observer.Messages[0].Value.Exception.Message);
            }

            [TestFixture]
            public sealed class When_calling_authorization_callback_with_empty_response : Given_a_registered_GoogleAuthorization
            {
                protected override RequestAuthorizationCode CreateCallback()
                {
                    RequestAuthorizationCode callback = uri => Observable.Empty<string>();
                    return callback;
                }

                [Test]
                public void Should_set_status_as_not_authorized()
                {
                    _sut.RequestAccessToken().Subscribe();
                    Assert.IsFalse(_sut.Status.IsAuthorized);
                }

                [Test]
                public void Should_set_status_as_not_processing()
                {
                    _sut.RequestAccessToken().Subscribe();
                    Assert.IsFalse(_sut.Status.IsProcessing);
                }
            }

            [TestFixture]
            public sealed class When_calling_authorization_callback_with_error_response : Given_a_registered_GoogleAuthorization
            {
                private Exception _expectedException;

                public override void SetUp()
                {
                    base.SetUp();
                    _expectedException = new Exception("Fail");
                }

                protected override RequestAuthorizationCode CreateCallback()
                {
                    RequestAuthorizationCode callback = uri => Observable.Throw<string>(_expectedException);
                    return callback;
                }

                [Test]
                public void Should_set_status_as_not_authorized()
                {
                    _sut.RequestAccessToken().Subscribe(i => { }, ex => { });
                    Assert.IsFalse(_sut.Status.IsAuthorized);
                }

                [Test]
                public void Should_set_status_as_not_processing()
                {
                    _sut.RequestAccessToken().Subscribe(i=>{},ex=> { });
                    Assert.IsFalse(_sut.Status.IsProcessing);
                }

                [Test]
                public void Should_pass_through_error()
                {
                    Exception actual = null;
                    _sut.RequestAccessToken().Subscribe(_ => { }, ex => { actual = ex; });
                    Assert.AreSame(_expectedException, actual);
                }
            }

            //when multiple concurrent requests for access token are made should only call call-back once
        }

        //Given an Authorized_GoogleAuthoization
        //  When requesting access token
        //  when requesting access token for modified resources
        //  when multiple concurrent requests for access token are made
        //Given a lapsed GoogleAuthoization
    }
}
// ReSharper restore InconsistentNaming
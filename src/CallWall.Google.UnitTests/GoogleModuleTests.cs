using System;
using CallWall.Contract.Contact;
using CallWall.Google.AccountConfiguration;
using CallWall.Google.Authorization;
using CallWall.Google.Authorization.Login;
using CallWall.Google.Providers;
using CallWall.Google.Providers.Contacts;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace CallWall.Google.UnitTests
{
    public abstract class Given_a_constructed_GoogleModule
    {
        #region Setup

        private Given_a_constructed_GoogleModule()
        {
        }

        private GoogleModule _hostModule;
        private Mock<IUnityContainer> _containerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _hostModule = new GoogleModule(_containerMock.Object);

        }

        #endregion

        [TestFixture]
        public sealed class When_Initialised : Given_a_constructed_GoogleModule
        {
            private Mock<ILoginController> _resolvedLoginController;
            public override void SetUp()
            {
                base.SetUp();
                _resolvedLoginController = new Mock<ILoginController>();
                _containerMock.Setup(c => c.Resolve(Match.Create<Type>(t => t == typeof (ILoginController)), null))
                    .Returns(_resolvedLoginController.Object);
                
                _hostModule.Initialize();
            }

            [Test]
            public void Should_register_GoogleAccountConfiguration_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IAccountConfiguration), typeof(GoogleAccountConfiguration), "GoogleAccountConfiguration", It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAuthorization_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IGoogleAuthorization), typeof(GoogleAuthorization), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_register_GoogleLoginView_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IGoogleLoginView), typeof(GoogleLoginView), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
            [Test]
            public void Should_register_LoginController_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(ILoginController), typeof(LoginController), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAccountSetupView_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IGoogleAccountSetupView), typeof(GoogleAccountSetupView), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAccountSetup_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IGoogleAccountSetup), typeof(GoogleAccountSetup), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_register_GoogleContactQueryProvider_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IContactQueryProvider), typeof(GoogleContactQueryProvider), "GoogleContactQueryProvider", It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            [Test]
            public void Should_start_LoginController()
            {
                _resolvedLoginController.Verify(lc=>lc.Start(), Times.Once());
            }
        }
    }
}

using CallWall.Windows.Contract;
using CallWall.Windows.Contract.Contact;
using CallWall.Windows.Google.AccountConfiguration;
using CallWall.Windows.Google.Authorization;
using CallWall.Windows.Google.Authorization.Login;
using CallWall.Windows.Google.Providers.Contacts;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Google.UnitTests
{
    public abstract class Given_a_constructed_GoogleModule
    {
        #region Setup

        private Given_a_constructed_GoogleModule()
        {
        }

        private GoogleModule _hostModule;
        private Mock<ITypeRegistry> _containerMock;
        private Mock<ILoginController> _resolvedLoginController;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<ITypeRegistry>();
            _resolvedLoginController = new Mock<ILoginController>();
            _hostModule = new GoogleModule(_containerMock.Object, ()=>_resolvedLoginController.Object);

        }

        #endregion

        [TestFixture]
        public sealed class When_Initialised : Given_a_constructed_GoogleModule
        {
            public override void SetUp()
            {
                base.SetUp();
                
                _hostModule.Initialize();
            }

            [Test]
            public void Should_register_GoogleAccountConfiguration_instance()
            {
                _containerMock.Verify(c => c.RegisterCompositeAsSingleton<IAccountConfiguration,GoogleAccountConfiguration>(), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAuthorization_instance()
            {
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<IGoogleAuthorization,GoogleAuthorization>(), Times.Once());
            }

            [Test]
            public void Should_register_GoogleLoginView_instance()
            {
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<IGoogleLoginView,GoogleLoginView>(), Times.Once());
            }
            
            [Test]
            public void Should_register_LoginController_instance()
            {
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<ILoginController,LoginController>(), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAccountSetupView_instance()
            {
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<IGoogleAccountSetupView,GoogleAccountSetupView>(), Times.Once());
            }

            [Test]
            public void Should_register_GoogleAccountSetupViewModel_instance()
            {
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<IGoogleAccountSetupViewModel,GoogleAccountSetupViewModel>(), Times.Once());
            }
            
            [Test]
            public void Should_register_GoogleContactQueryProvider_to_container_as_IGoogleContactQueryProvider_and_IContactQueryProvider()
            {
                _containerMock.Verify(c => c.RegisterCompositeAsSingleton<IContactQueryProvider,IGoogleContactQueryProvider, GoogleContactQueryProvider>(), Times.Once());
            }
            
            [Test]
            public void Should_start_LoginController()
            {
                _resolvedLoginController.Verify(lc=>lc.Start(), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
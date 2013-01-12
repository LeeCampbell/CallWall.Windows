using CallWall.Welcome;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Welcome
{
    public abstract class Given_a_constructed_WelcomeModule
    {
        private WelcomeModule _welcomeModule;
        private Mock<IUnityContainer> _containerMock;

        private Given_a_constructed_WelcomeModule()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _welcomeModule = new WelcomeModule(_containerMock.Object);
        }

        [TestFixture]
        public class When_Initialized : Given_a_constructed_WelcomeModule
        {
            private Mock<IWelcomeController> _welcomeControllerMock;

            public override void SetUp()
            {
                base.SetUp();
                _welcomeControllerMock = new Mock<IWelcomeController>();
                _containerMock.Setup(c => c.Resolve(typeof(IWelcomeController), (string)null)).Returns(_welcomeControllerMock.Object);
                _welcomeModule.Initialize();
            }

            [Test]
            public void Should_register_WelcomeController_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IWelcomeController), typeof(WelcomeController), (string)null, It.IsAny<TransientLifetimeManager>()));
            }

            [Test]
            public void Should_register_WelcomeView_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IWelcomeView), typeof(WelcomeView), (string)null, It.IsAny<TransientLifetimeManager>()));
            }

            [Test]
            public void Should_register_WelcomeStep1View_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IWelcomeStep1View), typeof(WelcomeStep1View), (string)null, It.IsAny<TransientLifetimeManager>()));
            }

            [Test]
            public void Should_Start_an_instance_of_WelcomeContoller()
            {
                _welcomeControllerMock.Verify(wc => wc.Start(), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
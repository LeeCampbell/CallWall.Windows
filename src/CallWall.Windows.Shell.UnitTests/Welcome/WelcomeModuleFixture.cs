using CallWall.Windows.Contract;
using CallWall.Windows.Shell.Welcome;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Shell.UnitTests.Welcome
{
    public abstract class Given_a_constructed_WelcomeModule
    {
        private WelcomeModule _welcomeModule;
        private Mock<ITypeRegistry> _typeRegistryMock;
        private Mock<IWelcomeController> _welcomeControllerMock;

        private Given_a_constructed_WelcomeModule()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _typeRegistryMock = new Mock<ITypeRegistry>();
            _welcomeControllerMock = new Mock<IWelcomeController>();
            _welcomeModule = new WelcomeModule(_typeRegistryMock.Object, () => _welcomeControllerMock.Object);
        }

        [TestFixture]
        public class When_Initialized : Given_a_constructed_WelcomeModule
        {
            public override void SetUp()
            {
                base.SetUp();
                
                _welcomeModule.Initialize();
            }

            [Test]
            public void Should_register_WelcomeController_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IWelcomeController, WelcomeController>(), Times.Once());
            }

            [Test]
            public void Should_register_WelcomeView_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IWelcomeView, WelcomeView>(), Times.Once());
            }

            [Test]
            public void Should_register_WelcomeStep1View_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IWelcomeStep1View, WelcomeStep1View>(), Times.Once());
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
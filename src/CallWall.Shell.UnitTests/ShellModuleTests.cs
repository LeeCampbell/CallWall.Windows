using CallWall.ProfileDashboard;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Shell.UnitTests
{
    public abstract class Given_a_constructed_ShellModule
    {
        #region Setup

        private Given_a_constructed_ShellModule()
        {
        }

        private ShellModule _hostModule;
        private Mock<IUnityContainer> _containerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _hostModule = new ShellModule(_containerMock.Object);
        }

        #endregion

        [TestFixture]
        public sealed class When_Initialised : Given_a_constructed_ShellModule
        {
            public override void SetUp()
            {
                base.SetUp();
                _hostModule.Initialize();
            }

            [Test]
            public void Should_register_ProfileActivatorAggregator_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IProfileActivatorAggregator), typeof(ProfileActivatorAggregator), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
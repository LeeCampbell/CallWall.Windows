using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests
{
    public abstract class Given_a_constructed_HostModule
    {
        #region Setup

        private Given_a_constructed_HostModule()
        {
        }

        private HostModule _hostModule;
        private Mock<IUnityContainer> _containerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _hostModule = new HostModule(_containerMock.Object);
        }

        #endregion

        [TestFixture]
        public sealed class When_Initialised : Given_a_constructed_HostModule
        {
            public override void SetUp()
            {
                base.SetUp();
                _hostModule.Initialize();
            }

            [Test]
            public void Should_register_SchedulerProvider_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(ISchedulerProvider), typeof(SchedulerProvider), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
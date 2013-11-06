using CallWall.Windows.Contract;
using CallWall.Windows.Shell.ProfileDashboard;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Shell.UnitTests
{
    public abstract class Given_a_constructed_ShellModule
    {
        #region Setup

        private Given_a_constructed_ShellModule()
        {
        }

        private ShellModule _hostModule;
        private Mock<ITypeRegistry> _containerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<ITypeRegistry>();
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
                _containerMock.Verify(c => c.RegisterTypeAsSingleton<IProfileActivatorAggregator,ProfileActivatorAggregator>(), Times.Once());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
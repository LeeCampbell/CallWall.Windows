using Moq;
using NUnit.Framework;

namespace CallWall.Core.UnitTests
{
    public abstract class Given_an_ILoggerFactory_instance
    {
        private Mock<ILoggerFactory> _loggerFactoryMock;

        [SetUp]
        public virtual void SetUp()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();
        }

        public class When_calling_CreateLogger_with_no_params : Given_an_ILoggerFactory_instance
        {
            [Test]
            public void Should_pass_type_that_calling_method_belongs_to()
            {
                var expected = this.GetType();
                var logger = new Mock<ILogger>().Object;
                _loggerFactoryMock.Setup(lf => lf.CreateLogger(expected))
                    .Returns(logger);

                var actual = _loggerFactoryMock.Object.CreateLogger();

                Assert.AreEqual(logger, actual);
                _loggerFactoryMock.Verify();
            }
        }
    }
}

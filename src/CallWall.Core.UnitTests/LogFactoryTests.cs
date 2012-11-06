using NUnit.Framework;
using log4net;

namespace CallWall.Core.UnitTests
{
    public abstract class Given_a_constructed_Logfactory
    {
        private Given_a_constructed_Logfactory()
        { }

        private ILoggerFactory _loggerFactory;

        [SetUp]
        public virtual void SetUp()
        {
            _loggerFactory = new LoggerFactory();
        }

        [TestFixture]
        public sealed class When_logger_created : Given_a_constructed_Logfactory
        {
            [Test]
            public void Should_return_Log4NetLogger_instance()
            {
                var logger = _loggerFactory.CreateLogger();
                Assert.AreEqual(typeof(Log4NetLogger), logger.GetType());
            }

            [Test]
            public void Should_return_instance_for_calling_type()
            {
                Assert.IsNull(LogManager.Exists(GetType().Name));
                var logger = _loggerFactory.CreateLogger();
                Assert.IsNotNull(LogManager.Exists(GetType().Name));
            }
        }
    }
}
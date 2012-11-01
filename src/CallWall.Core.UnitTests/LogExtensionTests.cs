using System;
using NUnit.Framework;


namespace CallWall.Core.UnitTests
{

    public abstract class Given_an_ILoggerInstance
    {
        private Given_an_ILoggerInstance()
        { }

        private FakeLogger _logger;

        [SetUp]
        public virtual void SetUp()
        {
            _logger = new FakeLogger();
        }

        public abstract class When_logging_via_extension_methods : Given_an_ILoggerInstance
        {
            private const string PlainMessage = "Plain message";
            private const string FormatMessage = "Value1:{0}, Value2:{1:o}";
            private const string Arg1 = "A";
            private static readonly DateTime Arg2 = new DateTime(2001, 12, 31, 13, 45, 27);

            private When_logging_via_extension_methods()
            { }

            protected abstract void Log(ILogger logger, Exception exception, string format, params object[] args);
            protected abstract void Log(ILogger logger, string format, params object[] args);
            protected abstract LogLevel ExpectedLevel { get; }

            [Test]
            public void Should_log_plain_messages_with_correct_Level()
            {
                Log(_logger, PlainMessage);
                Assert.AreEqual(ExpectedLevel, _logger.Level);
            }

            [Test]
            public void Should_log_plain_messages_with_message()
            {
                Log(_logger, PlainMessage);
                Assert.AreEqual(PlainMessage, _logger.Message);
            }

            [Test]
            public void Should_log_plain_messages_with_null_exception()
            {
                Log(_logger, PlainMessage);
                Assert.IsNull(_logger.Exception);
            }

            [Test]
            public void Should_log_formatted_messages_with_correct_Level()
            {
                Log(_logger, FormatMessage, Arg1, Arg2);
                Assert.AreEqual(ExpectedLevel, _logger.Level);
            }

            [Test]
            public void Should_log_formatted_messages_with_formatted_message()
            {
                var expected = string.Format(FormatMessage, Arg1, Arg2);
                Log(_logger, FormatMessage, Arg1, Arg2);
                Assert.AreEqual(expected, _logger.Message);
            }
            [Test]
            public void Should_log_formatted_messages_with_null_exception()
            {
                Log(_logger, FormatMessage, Arg1, Arg2);
                Assert.IsNull(_logger.Exception);
            }

            [Test]
            public void Should_log_exceptions_with_correct_Level()
            {
                Log(_logger, new Exception(), FormatMessage, Arg1, Arg2);
                Assert.AreEqual(ExpectedLevel, _logger.Level);
            }

            [Test]
            public void Should_log_exceptions_with_formatted_message()
            {
                var expected = string.Format(FormatMessage, Arg1, Arg2);
                Log(_logger, new Exception(), FormatMessage, Arg1, Arg2);
                Assert.AreEqual(expected, _logger.Message);
            }

            [Test]
            public void Should_log_exceptions_with_exception()
            {
                var expected = new Exception("Expected exception");
                Log(_logger, expected, FormatMessage, Arg1, Arg2);
                Assert.AreEqual(expected, _logger.Exception);
            }

            [TestFixture]
            public sealed class For_Fatal : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Fatal(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Fatal(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Fatal; }
                }
            }

            [TestFixture]
            public sealed class For_Error : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Error(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Error(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Error; }
                }
            }

            [TestFixture]
            public sealed class For_Warn : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Warn(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Warn(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Warn; }
                }
            }

            [TestFixture]
            public sealed class For_Info : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Info(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Info(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Info; }
                }
            }

            [TestFixture]
            public sealed class For_Debug : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Debug(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Debug(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Debug; }
                }
            }

            [TestFixture]
            public sealed class For_Trace : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Trace(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Trace(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Trace; }
                }
            }

            [TestFixture]
            public sealed class For_Verbose : When_logging_via_extension_methods
            {
                protected override void Log(ILogger logger, Exception exception, string format, params object[] args)
                {
                    logger.Verbose(exception, format, args);
                }

                protected override void Log(ILogger logger, string format, params object[] args)
                {
                    logger.Verbose(format, args);
                }

                protected override LogLevel ExpectedLevel
                {
                    get { return LogLevel.Verbose; }
                }
            }
        }

        private sealed class FakeLogger : ILogger
        {
            public void Write(LogLevel level, string message, Exception exception)
            {
                Level = level;
                Message = message;
                Exception = exception;
            }

            public LogLevel? Level { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
    }
}

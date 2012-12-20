using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
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

        public abstract class When_logging_messages_via_extension_methods : Given_an_ILoggerInstance
        {
            private const string PlainMessage = "Plain message";
            private const string FormatMessage = "Value1:{0}, Value2:{1:o}";
            private const string Arg1 = "A";
            private static readonly DateTime Arg2 = new DateTime(2001, 12, 31, 13, 45, 27);

            private When_logging_messages_via_extension_methods()
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
            public sealed class For_Fatal : When_logging_messages_via_extension_methods
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
            public sealed class For_Error : When_logging_messages_via_extension_methods
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
            public sealed class For_Warn : When_logging_messages_via_extension_methods
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
            public sealed class For_Info : When_logging_messages_via_extension_methods
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
            public sealed class For_Debug : When_logging_messages_via_extension_methods
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
            public sealed class For_Trace : When_logging_messages_via_extension_methods
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
            public sealed class For_Verbose : When_logging_messages_via_extension_methods
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

        [TestFixture]
        public sealed class When_logging_method_entry : Given_an_ILoggerInstance
        {
            [Test]
            public void Should_log_as_debug()
            {
                var model = new SampleLogConsumer(_logger);
                model.NoArgAction();
                Assert.AreEqual(LogLevel.Debug, _logger.Level);
            }

            [Test]
            public void Should_log_with_null_exception()
            {
                var model = new SampleLogConsumer(_logger);
                model.NoArgAction();
                Assert.IsNull(_logger.Exception);
            }

            [Test]
            public void Should_log_type_and_method_name()
            {
                var model = new SampleLogConsumer(_logger);
                model.NoArgAction();
                Assert.AreEqual("SampleLogConsumer.NoArgAction()", _logger.Message);
            }

            [Test]
            public void Should_log_arguments()
            {
                var arg1 = "A";
                var arg2 = new DateTime(2001, 12, 31, 13, 45, 27);
                var expected = string.Format("SampleLogConsumer.Action({0}, {1})", arg1, arg2);

                var model = new SampleLogConsumer(_logger);
                model.Action(arg1, arg2);
                Assert.AreEqual(expected, _logger.Message);
            }

            [Test]
            public void Should_log_placeholder_when_args_not_provided()
            {
                var arg1 = "A";
                var arg2 = new DateTime(2001, 12, 31, 13, 45, 27);
                var expected = string.Format("SampleLogConsumer.ActionNoArgsLogged(...)");

                var model = new SampleLogConsumer(_logger);
                model.ActionNoArgsLogged(arg1, arg2);

                Assert.AreEqual(expected, _logger.Message);
            }
        }

        [TestFixture]
        public sealed class When_logging_ObservableSequences : Given_an_ILoggerInstance
        {
            private string _logName;
            private Mock<ILogger> _loggerMock;
            public override void SetUp()
            {
                base.SetUp();
                _logName = "My sequence";
                _loggerMock = new Mock<ILogger>();
            }

            [Test]
            public void Should_log_subscription()
            {
                Observable.Never<Unit>()
                    .Log(_logger, _logName)
                    .Subscribe();

                Assert.AreEqual(LogLevel.Debug, _logger.Level);
                Assert.AreEqual(string.Format("{0}.Subscribe()", _logName), _logger.Message);
                Assert.IsNull(_logger.Exception);
            }

            [Test]
            public void Should_log_OnNext()
            {
                var value = 1;
                Observable.Return(value)
                    .Concat(Observable.Never<int>())
                    .Log(_logger, _logName)
                    .Subscribe();

                Assert.AreEqual(LogLevel.Debug, _logger.Level);
                Assert.AreEqual(string.Format("{0}.OnNext({1})", _logName, value), _logger.Message);
                Assert.IsNull(_logger.Exception);
            }

            [Test]
            public void Should_log_OnError()
            {
                var ex = new Exception("Message");
                var expectedMessage = string.Format("{0}.OnError({1})", _logName, ex);

                Observable.Throw<int>(ex)
                    .Log(_loggerMock.Object, _logName)
                    .Subscribe(i => { }, e => { });

                _loggerMock.Verify(l => l.Write(LogLevel.Debug, expectedMessage, null), Times.Once());
            }

            [Test]
            public void Should_log_OnCompleted()
            {
                var expectedMessage = string.Format("{0}.OnCompleted()", _logName);
                Observable.Empty<int>()
                    .Log(_loggerMock.Object, _logName)
                    .Subscribe();

                _loggerMock.Verify(l => l.Write(LogLevel.Debug, expectedMessage, null), Times.Once());
            }

            [Test]
            public void Should_log_Disposal_of_subscription()
            {
                Observable.Never<int>()
                    .Log(_logger, _logName)
                    .Subscribe()
                    .Dispose();

                Assert.AreEqual(LogLevel.Debug, _logger.Level);
                Assert.AreEqual(string.Format("{0}.Dispose()", _logName), _logger.Message);
                Assert.IsNull(_logger.Exception);
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

            public LogLevel? Level { get; private set; }
            public string Message { get; private set; }
            public Exception Exception { get; private set; }
        }
    }

    public sealed class SampleLogConsumer
    {
        private readonly ILogger _logger;

        public SampleLogConsumer(ILogger logger)
        {
            _logger = logger;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void NoArgAction()
        {
            _logger.MethodEntry();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Action(string s, DateTime dateTime)
        {
            _logger.MethodEntry(s, dateTime);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ActionNoArgsLogged(string s, DateTime dateTime)
        {
            _logger.MethodEntry();
        }
    }
}
// ReSharper restore InconsistentNaming
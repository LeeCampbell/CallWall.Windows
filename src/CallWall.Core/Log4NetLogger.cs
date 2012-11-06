using log4net;
using log4net.Core;
using Microsoft.Practices.Prism.Logging;
using System;

namespace CallWall
{
    /// <summary>
    /// Implementation of the CallWall <see cref="ILogger"/> interface and Prism <see cref="ILoggerFacade"/> interface.
    /// </summary>
    public sealed class Log4NetLogger : ILogger, ILoggerFacade
    {
        private readonly ILog _log;

        public Log4NetLogger(Type callingType)
        {
            if (callingType == null) throw new ArgumentNullException("callingType");
            _log = LogManager.GetLogger(callingType.Name);
        }

        /// <summary>
        /// Writes the message and optional exception to the log for the given level.
        /// </summary>
        /// <param name="level">The Logging level to apply. Useful for filtering messages</param>
        /// <param name="message">The message to be logged</param>
        /// <param name="exception">An optional exception to be logged with the message</param>
        /// <remarks>
        /// It is preferable to use the <see cref="ILogger"/> extension methods found in the <see cref="LoggerExtensions"/> static type.
        /// </remarks>
        public void Write(LogLevel level, string message, Exception exception)
        {
            _log.Logger.Log(null, ToLog4Net(level), message, exception);
        }


        /// <summary>
        /// Explicit implementation of the Prism <see cref="ILoggerFacade"/> interface
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="category">The category to log to</param>
        /// <param name="priority">Ignored in this implementation</param>
        void ILoggerFacade.Log(string message, Category category, Priority priority)
        {
            _log.Logger.Log(null, ToLog4Net(category), message, null);
        }

        private static Level ToLog4Net(Category category)
        {
            switch (category)
            {
                case Category.Debug:
                    return Level.Debug;
                case Category.Exception:
                    return Level.Error;
                case Category.Info:
                    return Level.Info;
                case Category.Warn:
                    return Level.Warn;
                default:
                    throw new ArgumentOutOfRangeException("category");
            }
        }

        private static Level ToLog4Net(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    return Level.Verbose;
                case LogLevel.Trace:
                    return Level.Trace;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }
    }
}
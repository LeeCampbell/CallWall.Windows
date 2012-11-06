using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Globalization;

namespace CallWall
{
    public static class LoggerExtensions
    {
        [StringFormatMethod("format")]
        public static void Fatal(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Fatal, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Fatal(null, format, args);
        }

        [StringFormatMethod("format")]
        public static void Error(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Error, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Error(null, format, args);
        }

        [StringFormatMethod("format")]
        public static void Info(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Info, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Info(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Info(null, format, args);
        }

        [StringFormatMethod("format")]
        public static void Warn(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Warn, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Warn(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Warn(null, format, args);
        }

        [StringFormatMethod("format")]
        public static void Debug(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Debug, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Debug(null, format, args);
        }

        [StringFormatMethod("format")]
        public static void Trace(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Trace, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Trace(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Trace(null, format, args);
        }

        /// <summary>
        /// Logs messages as Verbose, the lowest level.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        [StringFormatMethod("format")]
        public static void Verbose(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = string.Format(CultureInfo.CurrentCulture, format, args);
            logger.Write(LogLevel.Verbose, formattedMessage, exception);
        }
        [StringFormatMethod("format")]
        public static void Verbose(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Verbose(null, format, args);
        }

        /// <summary>
        /// Logs the entry to a method as a string like "MyType.MyMethod(1, ABC)". 
        /// Ensure the method being logged is not in-lined by the compiler/jitter with the
        /// [MethodImpl(MethodImplOptions.NoInlining)] attribute.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="args"></param>
        public static void MethodEntry(this ILogger logger, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var stackTrace = new StackTrace();
            var method = stackTrace.GetFrame(1).GetMethod();
            var type = method.DeclaringType;
            var typeName = string.Empty;
            if (type != null) typeName = type.Name;
            string parenth = "()";
            var parameterDefinitions = method.GetParameters();
            if (parameterDefinitions.Length > 0)
            {
                if (args == null || args.Length == 0)
                {
                    parenth = "(...)";
                }
                else
                {
                    var values = string.Join(", ", args);
                    parenth = string.Format(CultureInfo.CurrentCulture, "({0})", values);
                }
            }

            logger.Debug("{0}.{1}{2}", typeName, method.Name, parenth);
        }
    }
}
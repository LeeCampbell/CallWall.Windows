using System;
using System.Diagnostics;

namespace CallWall
{
    /// <summary>
    /// Factory for creating <see cref="ILogger"/> instances.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Factory method to create an <see cref="ILogger"/> instance.
        /// </summary>
        /// <returns>Returns an instance of an <see cref="ILogger"/> for the calling type.</returns>
        ILogger CreateLogger(Type loggedType);
    }

    public static class LoggerFactoryExtensions
    {
        public static ILogger CreateLogger(this ILoggerFactory loggerFactory)
        {
            var callersStackFrame = new StackFrame(1);
            var callerMethod = callersStackFrame.GetMethod();
            var callingType = callerMethod.ReflectedType;
            return loggerFactory.CreateLogger(callingType);
        }
    }
}
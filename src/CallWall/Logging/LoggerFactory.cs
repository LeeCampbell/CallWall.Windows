using System.Diagnostics;

namespace CallWall.Logging
{
    public sealed class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            var callersStackFrame = new StackFrame(1);
            var callerMethod = callersStackFrame.GetMethod();
            var callingType = callerMethod.ReflectedType;
            return new Log4NetLogger(callingType);
        }
    }
}
using System;

namespace CallWall.Testing
{
    public sealed class NullLogger : ILogger
    {
        public static readonly ILogger Instance = new NullLogger();

        private NullLogger()
        {}

        public void Write(LogLevel level, string message, Exception exception)
        {}
    }
}
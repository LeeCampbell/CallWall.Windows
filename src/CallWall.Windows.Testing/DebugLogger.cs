using System;
using System.Diagnostics;

namespace CallWall.Windows.Testing
{
    public sealed class DebugLogger : ILogger
    {
        public void Write(LogLevel level, string message, Exception exception)
        {
            Debug.WriteLine("{0} {1} {2}", level, message, exception);
        }
    }
}
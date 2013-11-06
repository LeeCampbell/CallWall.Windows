using System;

namespace CallWall.Windows.Contract.Calendar
{
    public interface ICalendarEvent
    {
        DateTimeOffset Start { get; }
        DateTimeOffset End { get; }

        string Name { get; }
        string Description { get; }

        /// <summary>
        /// The related provider of the message.
        /// </summary>
        IProviderDescription Provider { get; }
    }
}
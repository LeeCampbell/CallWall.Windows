using System.Reactive.Concurrency;

namespace CallWall
{
    public interface ISchedulerProvider
    {
        /// <summary>
        /// Provides access to scheduling onto the UI Dispatcher. 
        /// </summary>
        IScheduler Dispatcher { get; }

        /// <summary>
        /// Provides concurrent scheduling. Will use the thread pool or the task pool if available.
        /// </summary>
        IScheduler Concurrent { get; }

        /// <summary>
        /// Provides concurrent scheduling for starting long running tasks. Will use a new thread or a long running task if available. Can be used to run loops more efficiently than using recursive scheduling.
        /// </summary>
        ISchedulerLongRunning LongRunning { get; }

        /// <summary>
        /// Provides support for scheduling periodic tasks. Can be used to run timers more efficiently than using recursive scheduling.
        /// </summary>
        ISchedulerPeriodic Periodic { get; }

        /// <summary>
        /// Creates an instance of an Event-Loop scheduler. Useful when a dedicated thread is appropriate.
        /// </summary>
        /// <param name="name">The Name of the thread.</param>
        /// <returns>An instance of an <see cref="IEventLoopScheduler"/>.</returns>
        IEventLoopScheduler CreateEventLoopScheduler(string name);
    }
}
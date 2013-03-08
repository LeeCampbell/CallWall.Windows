using System.Reactive.Concurrency;

namespace CallWall
{
    public interface ISchedulerProvider
    {
        /// <summary>
        /// Provides asynchronous scheduling without introducing concurrency. In Client applications this will generally use the dispatcher, else probably the <see cref="CurrentThreadScheduler"/>.
        /// </summary>
        IScheduler Async { get; }

        /// <summary>
        /// Provides concurrent scheduling. Will use the thread pool or the task pool if available.
        /// </summary>
        IScheduler Concurrent { get; }

        /// <summary>
        /// Provides concurrent scheduling for long running tasks. Will use a new thread or a long running task if available.
        /// </summary>
        ISchedulerLongRunning LongRunning { get; }

        /// <summary>
        /// Creates an instance of an Event-Loop scheduler. Useful when a dedicated thread is appropriate.
        /// </summary>
        /// <param name="name">The Name of the thread.</param>
        /// <returns>An instance of an <see cref="IEventLoopScheduler"/>.</returns>
        IEventLoopScheduler CreateEventLoopScheduler(string name);
    }
}
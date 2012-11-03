using System.Reactive.Concurrency;

namespace CallWall
{
    public interface ISchedulerProvider
    {
        /// <summary>
        /// Provides asynchronous scheduling without introducing concurrency. In Client applications this will generally use the dispatcher
        /// </summary>
        IScheduler Async { get; }

        /// <summary>
        /// Provides concurrent scheduling. Will use the thread pool or the task pool if available.
        /// </summary>
        IScheduler Concurrent { get; }      //Default for Concurrency

        /// <summary>
        /// Provides concurrent scheduling for long running tasks. Will use a new thread or a long running task if available.
        /// </summary>
        IScheduler LongRunning { get; }
    }
}
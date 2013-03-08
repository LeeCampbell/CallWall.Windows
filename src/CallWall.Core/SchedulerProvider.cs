using System.Reactive.Concurrency;

namespace CallWall
{
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        private readonly IScheduler _dispatcherScheduler;

        public SchedulerProvider()
        {
            var currentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            _dispatcherScheduler = new DispatcherScheduler(currentDispatcher);
        }

        public IScheduler Async
        {
            get { return _dispatcherScheduler; }
        }

        public IScheduler Concurrent
        {
            get { return TaskPoolScheduler.Default; }
        }

        public ISchedulerLongRunning LongRunning
        {
            get { return TaskPoolScheduler.Default; }
        }

        public IEventLoopScheduler CreateEventLoopScheduler(string name)
        {
            return new EventLoopSchedulerWrapper(name);
        }
    }
}
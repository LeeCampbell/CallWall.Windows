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
        public IScheduler Concurrent
        {
            get { return _dispatcherScheduler; }
        }

        public IScheduler LongRunning
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler Async
        {
            get { return ThreadPoolScheduler.Instance; }
        }
    }
}
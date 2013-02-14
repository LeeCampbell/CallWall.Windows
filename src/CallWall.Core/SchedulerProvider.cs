using System.Reactive.Concurrency;
using System.Threading;

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
            //get { return ThreadPoolScheduler.Instance; }
            get { return TaskPoolScheduler.Default; }
        }

        public IScheduler LongRunning
        {
            get { return new NewThreadScheduler(ts => new Thread(ts) { IsBackground = true }); }
        }
    }
}
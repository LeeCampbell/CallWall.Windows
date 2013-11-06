using System.Reactive.Concurrency;

namespace CallWall.Windows
{
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        private readonly IScheduler _dispatcherScheduler;

        public SchedulerProvider()
        {
            var currentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            _dispatcherScheduler = new DispatcherScheduler(currentDispatcher);
        }

        public IScheduler Dispatcher
        {
            get { return _dispatcherScheduler; }
        }


        public IScheduler Concurrent
        {
            get { return TaskPoolScheduler.Default; }
        }

        public ISchedulerLongRunning LongRunning
        {
            get { return TaskPoolScheduler.Default.AsLongRunning(); }
        }

        public ISchedulerPeriodic Periodic
        {
            get { return TaskPoolScheduler.Default.AsPeriodic(); }
        }

        public IEventLoopScheduler CreateEventLoopScheduler(string name)
        {
            return new EventLoopSchedulerWrapper(name);
        }
    }
}
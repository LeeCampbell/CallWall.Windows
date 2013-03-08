using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace CallWall.Testing
{
    public sealed class TestSchedulerProvider : ISchedulerProvider
    {
        private readonly TestScheduler _async = new TestScheduler();
        private readonly TestScheduler _concurrent = new TestScheduler();
        private readonly LongRunningTestScheudler _longRunning = new LongRunningTestScheudler(new TestScheduler());

        IScheduler ISchedulerProvider.Async
        {
            get { return _async; }
        }
        public TestScheduler Async
        {
            get { return _async; }
        }

        IScheduler ISchedulerProvider.Concurrent
        {
            get { return _concurrent; }
        }
        public TestScheduler Concurrent
        {
            get { return _concurrent; }
        }

        ISchedulerLongRunning ISchedulerProvider.LongRunning
        {
            get { return _longRunning; }
        }
        public LongRunningTestScheudler LongRunning
        {
            get { return _longRunning; }
        }

        public IEventLoopScheduler CreateEventLoopScheduler(string name)
        {
            return new EventLoopTestScheduler(new TestScheduler());
        }
    }
}
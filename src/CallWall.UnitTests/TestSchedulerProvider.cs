using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace CallWall.UnitTests
{
    public sealed class TestSchedulerProvider : ISchedulerProvider
    {
        private readonly TestScheduler _async = new TestScheduler();
        private readonly TestScheduler _concurrent = new TestScheduler();
        private readonly TestScheduler _longRunning = new TestScheduler();

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

        IScheduler ISchedulerProvider.LongRunning
        {
            get { return _longRunning; }
        }
        public TestScheduler LongRunning
        {
            get { return _longRunning; }
        }
    }
}
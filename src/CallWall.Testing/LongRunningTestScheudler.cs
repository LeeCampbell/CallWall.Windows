using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Microsoft.Reactive.Testing;

namespace CallWall.Testing
{
    public sealed class LongRunningTestScheudler : ISchedulerLongRunning
    {
        private readonly TestScheduler _underlying;

        public LongRunningTestScheudler(TestScheduler underlying)
        {
            _underlying = underlying;
        }

        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            var cancellation = new BooleanDisposable();
            Func<IScheduler, TState, IDisposable> fun = (sched, s) =>
                {
                    action(s, cancellation);
                    return cancellation;
                };
            return _underlying.Schedule(state, fun);
        }
    }
}
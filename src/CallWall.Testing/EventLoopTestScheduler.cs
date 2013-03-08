using System;
using System.Reactive;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace CallWall.Testing
{
    public sealed class EventLoopTestScheduler : IEventLoopScheduler
    {
        private readonly TestScheduler _testScheduler;

        public EventLoopTestScheduler(TestScheduler testScheduler)
        {
            _testScheduler = testScheduler;
        }

        public bool IsDisposed { get; private set; }

        public bool IsEnabled { get { return _testScheduler.IsEnabled; } }

        public DateTimeOffset Now { get { return _testScheduler.Now; } }

        public long Clock { get { return _testScheduler.Clock; } }


        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _testScheduler.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _testScheduler.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _testScheduler.Schedule(state, action);
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void AdvanceBy(long time)
        {
            _testScheduler.AdvanceBy(time);
        }
        public void AdvanceTo(long time)
        {
            _testScheduler.AdvanceTo(time);
        }
        public ITestableObservable<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages)
        {
            return _testScheduler.CreateColdObservable<T>(messages);
        }
        public ITestableObservable<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages)
        {
            return _testScheduler.CreateHotObservable<T>(messages);
        }
        public ITestableObserver<T> CreateObserver<T>()
        {
            return _testScheduler.CreateObserver<T>();
        }
        public IDisposable ScheduleAbsolute<T>(T state, long dueTime, Func<IScheduler, T, IDisposable> action)
        {
            return _testScheduler.ScheduleAbsolute<T>(state, dueTime, action);
        }
        public IDisposable ScheduleAbsolute<T>(long dueTime, Action action)
        {
            return _testScheduler.ScheduleAbsolute(dueTime, action);
        }
        public IDisposable ScheduleRelative<T>(T state, long dueTime, Func<IScheduler, T, IDisposable> action)
        {
            return _testScheduler.ScheduleRelative<T>(state, dueTime, action);
        }
        public IDisposable ScheduleRelative<T>(long dueTime, Action action)
        {
            return _testScheduler.ScheduleRelative(dueTime, action);
        }
        public void Sleep(long time)
        {
            _testScheduler.Sleep(time);
        }
        public void Stop()
        {
            _testScheduler.Stop();
        }
        public void Start()
        {
            _testScheduler.Start();
        }
        public ITestableObserver<T> Start<T>(Func<IObservable<T>> create)
        {
            return _testScheduler.Start<T>(create);
        }
        public ITestableObserver<T> Start<T>(Func<IObservable<T>> create, long disposed)
        {
            return _testScheduler.Start<T>(create, disposed);
        }
        public ITestableObserver<T> Start<T>(Func<IObservable<T>> create, long created, long subscribed, long disposed)
        {
            return _testScheduler.Start<T>(create, created, subscribed, disposed);
        }
        public IStopwatch StartStopwatch()
        {
            return _testScheduler.StartStopwatch();
        }
    }
}
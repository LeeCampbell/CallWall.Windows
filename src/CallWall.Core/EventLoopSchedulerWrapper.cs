using System;
using System.Reactive.Concurrency;
using System.Threading;

namespace CallWall
{
    /// <summary>
    /// Wraps an <see cref="EventLoopScheduler"/> so it can be exposed via the <see cref="IDisposable"/>/<see cref="IScheduler"/> composite interface.
    /// </summary>
    public sealed class EventLoopSchedulerWrapper : IEventLoopScheduler
    {
        private readonly EventLoopScheduler _els;
        private readonly string _threadName;
        private readonly bool _isBackgroundThread;

        public EventLoopSchedulerWrapper(string name)
        {
            _threadName = name;
            _isBackgroundThread = true;
            _els = new EventLoopScheduler((ThreadStart ts) => new Thread(ts) {Name = _threadName, IsBackground = _isBackgroundThread});
        }

        public bool IsBackgroundThread
        {
            get { return _isBackgroundThread; }
        }

        public string ThreadName
        {
            get { return _threadName; }
        }

        public DateTimeOffset Now
        {
            get { return _els.Now; }
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _els.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _els.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _els.Schedule(state, action);
        }
        
        public void Dispose()
        {
            _els.Dispose();
        }
    }
}
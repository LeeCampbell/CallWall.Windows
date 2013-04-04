using System;
using System.Collections;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace CallWall
{
    public static class ObservableExtensions
    {
        public static IObservable<T> LazyConcat<T>(params IObservable<T>[] sources)
        {
            return Observable.Create<T>(o =>
            {
                var cancelFlag = new BooleanDisposable();
                var enumerator = sources.GetEnumerator();

                Func<IScheduler, IEnumerator, IDisposable> loop = null;
                loop = (s, e) =>
                {
                    var subscription = Disposable.Empty;
                    if (!cancelFlag.IsDisposed && e.MoveNext())
                    {
                        var cur = (IObservable<T>)e.Current;
                        subscription = cur.Subscribe(
                            i => o.OnNext(i),
                            ex => o.OnError(ex),
                            () => { s.Schedule(e, loop); });
                    }
                    return subscription;
                };

                //var scheduled = ImmediateScheduler.Instance.Schedule(enumerator, loop);
                var scheduled = CurrentThreadScheduler.Instance.Schedule(enumerator, loop);

                return new CompositeDisposable(cancelFlag, scheduled);
            });
        }

        //TODO: Could potentially upgrade to using tasks/Await-LC

        public static IObservable<byte> ToObservable(
            this Stream source,
            int bufferSize,
            IScheduler scheduler)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (bufferSize < 1) throw new ArgumentOutOfRangeException("bufferSize");
            if (scheduler == null) throw new ArgumentNullException("scheduler");

            var bytes = Observable.Create<byte>(o =>
                {
                    var initialState = new StreamReaderState(source, bufferSize);
                    var currentStateSubscription = new SerialDisposable();
                    Action<StreamReaderState, Action<StreamReaderState>> iterator =
                        (state, self) =>
                        currentStateSubscription.Disposable = state.ReadNext().Subscribe(
                            bytesRead =>
                            {
                                for (int i = 0; i < bytesRead; i++)
                                {
                                    o.OnNext(state.Buffer[i]);
                                }
                                if (bytesRead > 0)
                                    self(state);
                                else
                                    o.OnCompleted();
                            },
                            o.OnError);
                    var scheduledWork = scheduler.Schedule(initialState, iterator);
                    return new CompositeDisposable(currentStateSubscription, scheduledWork);
                });
            return Observable.Using(() => source, _ => bytes);
        }

        private sealed class StreamReaderState
        {
            private readonly Stream _source;
            private readonly int _bufferSize;
            public StreamReaderState(Stream source, int bufferSize)
            {
                _source = source;
                _bufferSize = bufferSize;
                Buffer = new byte[bufferSize];
            }
            public IObservable<int> ReadNext()
            {
                return _source.ReadAsync(Buffer, 0, _bufferSize).ToObservable();
            }
            public byte[] Buffer { get; private set; }
        }
    }
}
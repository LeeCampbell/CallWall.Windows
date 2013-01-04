using System;
using System.Collections;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CallWall
{
    public static class ObservableExtensions
    {
        //public static IObservable<T> Log<T>(this IObservable<T> source, ILogger logger, string name)
        //{
        //    return Observable.Create<T>(o =>
        //                                    {
        //                                        logger.Trace("[->]{0}.Subscribe()", name);
        //                                        var subscription = source.Do
        //                                            (
        //                                                i => logger.Trace("[0]{0}.OnNext({1})", name, i),
        //                                                ex => logger.Trace("[X]{0}.OnError({1})", name, ex),
        //                                                () => logger.Trace("[|]{0}.OnComplete()", name)
        //                                            )
        //                                            .Subscribe(o);
        //                                        var loggedDisposal = Disposable.Create(() => { logger.Trace("[<-]{0}.Dispose()", name); });
        //                                        return new CompositeDisposable(loggedDisposal, subscription);
        //                                    });
        //}

        //public static IObservable<T> LazyConcat<T>(params IObservable<T>[] sources)
        //{
        //    return Observable.Create<T>(o =>
        //                                    {
        //                                        var cancelFlag = new BooleanDisposable();
        //                                        var enumerator = sources.GetEnumerator();

        //                                        Func<IScheduler, IEnumerator, IDisposable> loop = null;
        //                                        loop = (s, e) =>
        //                                                   {
        //                                                       IDisposable subscription = Disposable.Empty;
        //                                                       if (!cancelFlag.IsDisposed && e.MoveNext())
        //                                                       {
        //                                                           var cur = (IObservable<T>)e.Current;
        //                                                           subscription = cur.Subscribe(
        //                                                               i => o.OnNext(i),
        //                                                               ex => o.OnError(ex),
        //                                                               () => { s.Schedule(e, loop); });
        //                                                       }
        //                                                       return subscription;
        //                                                   };

        //                                        var scheduled = ImmediateScheduler.Instance.Schedule(enumerator, loop);

        //                                        return new CompositeDisposable(cancelFlag, scheduled);
        //                                    });
        //}

        //TODO: Could potentially upgrade to using tasks/Await-LC

        public static IObservable<byte> ToObservable(
            this Stream source,
            int buffersize,
            IScheduler scheduler)
        {
            var bytes = Observable.Create<byte>(o =>
                {
                    var initialState = new StreamReaderState(source, buffersize);
                    var currentStateSubscription = new SerialDisposable();
                    Action<StreamReaderState, Action<StreamReaderState>> iterator =
                        (state, self) =>
                        currentStateSubscription.Disposable = state.ReadNext()
                                                                    .Subscribe(
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
            private readonly int _bufferSize;
            private readonly Func<byte[], int, int, IObservable<int>> _factory;
            public StreamReaderState(Stream source, int bufferSize)
            {
                _bufferSize = bufferSize;
                _factory = Observable.FromAsyncPattern<byte[], int, int, int>(
                    source.BeginRead,
                    source.EndRead);
                Buffer = new byte[bufferSize];
            }
            public IObservable<int> ReadNext()
            {
                return _factory(Buffer, 0, _bufferSize);
            }
            public byte[] Buffer { get; set; }
        }
    }
}
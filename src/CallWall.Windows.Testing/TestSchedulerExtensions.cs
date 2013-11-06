using System.Reactive;
using Microsoft.Reactive.Testing;

namespace CallWall.Windows.Testing
{
    public static class TestSchedulerExtensions
    {
        public static ITestableObservable<T> CreateSingleValueColdObservable<T>(this TestScheduler testScheduler, T value)
        {
            return testScheduler.CreateColdObservable(
                new Recorded<Notification<T>>(1, Notification.CreateOnNext(value)),
                new Recorded<Notification<T>>(1, Notification.CreateOnCompleted<T>())
                );
        }
    }
}
using System.Reactive.Concurrency;

namespace CallWall
{
    public interface ISchedulerProvider
    {
        //IDispatcherScheduler Dispatcher { get; }

        IScheduler Async { get; } 

        IScheduler LongRunning { get; }   //Only Long running stuff

        IScheduler Concurrent { get; }  //Default for Concurrency
    }

    //public interface IDispatcherScheduler : IScheduler
    //{
    //    void Schedule(Action action, DispatcherPriority dispatcherPriority);
    //    void Invoke(Action action);
    //}
}
using System.Reactive.Concurrency;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests
{
    [TestFixture]
    public sealed class SchedulerProviderTests
    {
        private SchedulerProvider _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SchedulerProvider();
        }

        [Test]
        public void Should_return_DispatcherScheduler_from_Dispatcher()
        {
            Assert.IsInstanceOf<DispatcherScheduler>(_sut.Dispatcher);
        }

        [Test]
        public void Should_return_TaskPoolScheduler_from_Concurrent_property()
        {
            Assert.IsInstanceOf<TaskPoolScheduler>(_sut.Concurrent);
        }

        [Test]
        public void Should_return_TaskPoolScheduler_from_LongRunning_property()
        {
            Assert.IsInstanceOf<TaskPoolScheduler>(_sut.LongRunning);
        }

        [Test]
        public void Should_return_TaskPoolScheduler_from_Periodic_property()
        {
            Assert.IsInstanceOf<TaskPoolScheduler>(_sut.Periodic);
        }

        [Test]
        public void Should_return_EventLoopScheduler_from_Create()
        {
            var expectedName = "TheName";
            var eventLoopScheduler = _sut.CreateEventLoopScheduler(expectedName);

            Assert.IsTrue(eventLoopScheduler.IsBackgroundThread);
            Assert.AreEqual(expectedName, eventLoopScheduler.ThreadName);
        }
    }
}

// ReSharper restore InconsistentNaming
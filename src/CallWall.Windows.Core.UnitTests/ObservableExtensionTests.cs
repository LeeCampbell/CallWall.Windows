using System.IO;
using System.Linq;
using System.Reactive;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Core.UnitTests
{
    public abstract class Given_a_Stream_of_data
    {
        private byte[] _expected;
        private Stream _stream;

        [SetUp]
        public virtual void SetUp()
        {
            _expected = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            _stream = new MemoryStream(_expected);
        }

        [TestFixture]
        public class When_converted_to_an_observable_sequence_of_bytes : Given_a_Stream_of_data
        {
            [Test]
            public void Should_return_all_bytes_in_same_order([Range(1,20)]int bufferSize)
            {
                var scheduler = new TestScheduler();
                var observer = scheduler.CreateObserver<byte>();
                using (_stream.ToObservable(bufferSize, scheduler).Subscribe(observer))
                {
                    scheduler.Start();

                    var actual = observer.Messages.Select(rn => rn.Value)
                        .Where(n => n.Kind == NotificationKind.OnNext)
                        .Select(n => n.Value)
                        .ToList();
                    CollectionAssert.AreEquivalent(_expected, actual);
                }
            }

            [Test]
            public void Should_throw_is_Source_is_null()
            {
                _stream = null;
                var ex = Assert.Throws<System.ArgumentNullException>(() => _stream.ToObservable(1, new TestScheduler()));
                Assert.AreEqual("source", ex.ParamName);
            }

            [Test]
            public void Should_reject_buffer_of_zero()
            {
                var scheduler = new TestScheduler();
                var ex= Assert.Throws<System.ArgumentOutOfRangeException>(() => _stream.ToObservable(0, scheduler));
                Assert.AreEqual("bufferSize", ex.ParamName);
            }

            [Test]
            public void Should_throw_is_Scheduler_is_null()
            {
                var ex = Assert.Throws<System.ArgumentNullException>(() => _stream.ToObservable(1, null));
                Assert.AreEqual("scheduler", ex.ParamName);
            }
        }
    }
}

// ReSharper restore InconsistentNaming
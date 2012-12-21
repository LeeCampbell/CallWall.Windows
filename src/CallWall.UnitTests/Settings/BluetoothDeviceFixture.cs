using System.Reactive;
using System.Reactive.Linq;
using CallWall.Services;
using CallWall.Settings.Bluetooth;
using InTheHand.Net.Bluetooth;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_BluetoothDevice
    {
        private Given_a_constructed_BluetoothDevice()
        { }

        private Mock<IBluetoothService> _bluetoothServiceMock;
        private TestSchedulerProvider _testSchedulerProvider;
        private Mock<IBluetoothDeviceInfo> _bluetoothDeviceInfoMock;
        private BluetoothDevice _sut;

        private const string _expectedName = "My Bt device";
        private readonly byte[] _expectedAddress = new byte[] { 1, 2, 3, 4, 5, 6 };

        [SetUp]
        public virtual void Setup()
        {
            _bluetoothServiceMock = new Mock<IBluetoothService>();
            _testSchedulerProvider = new TestSchedulerProvider();
            _bluetoothDeviceInfoMock = new Mock<IBluetoothDeviceInfo>();

            _sut = new BluetoothDevice(_bluetoothDeviceInfoMock.Object, _bluetoothServiceMock.Object,
                                       _testSchedulerProvider);


            _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceAddress).Returns(_expectedAddress);
            _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceName).Returns(_expectedName);
        }

        [Test]
        public void Should_return_underlying_DeviceName()
        {
            Assert.AreEqual(_expectedName, _sut.Name);
        }

        //TODO: Figure out what the NUnit enum parameter generator is -LC
        [TestCase(DeviceClass.SmartPhone)]
        [TestCase(DeviceClass.Phone)]
        [TestCase(DeviceClass.CellPhone)]
        [TestCase(DeviceClass.Computer)]
        [TestCase(DeviceClass.LaptopComputer)]
        public void Should_return_underlying_DeviceType(DeviceClass deviceClass)
        {
            var expectedDeviceType = BluetoothDeviceType.Create(deviceClass);
            _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceType).Returns(expectedDeviceType);

            Assert.AreEqual(expectedDeviceType, _sut.DeviceType);
        }

        public abstract class When_device_is_in_an_idle_state : Given_a_constructed_BluetoothDevice
        {
            private When_device_is_in_an_idle_state()
            { }

            public override void Setup()
            {
                base.Setup();
                Assume.That(_sut.Status.IsProcessing, Is.False);
                Assume.That(_sut.Status.HasError, Is.False);
            }

            [TestFixture]
            public sealed class When_device_is_authenticated : When_device_is_in_an_idle_state
            {
                public override void Setup()
                {
                    base.Setup();
                    _bluetoothDeviceInfoMock.Setup(btd => btd.IsAuthenticated).Returns(true);
                }

                [Test]
                public void Should_not_be_able_to_pair_the_device()
                {
                    Assert.IsFalse(_sut.PairDeviceCommand.CanExecute());
                }

                [Test]
                public void Should_be_able_to_remove_the_device()
                {
                    Assert.IsTrue(_sut.RemoveDeviceCommand.CanExecute());
                }

                [Test, Ignore]
                public void Should_be_able_to_test_the_device_connection()
                {
                    Assert.IsTrue(_sut.TestDeviceCommand.CanExecute());
                }


            }

            [TestFixture]
            public sealed class When_device_is_not_authenticated : When_device_is_in_an_idle_state
            {
                public override void Setup()
                {
                    base.Setup();
                    _bluetoothDeviceInfoMock.Setup(btd => btd.IsAuthenticated).Returns(false);
                }

                [Test]
                public void Should_be_able_to_pair_the_device()
                {
                    Assert.IsTrue(_sut.PairDeviceCommand.CanExecute());
                }

                [Test]
                public void Should_not_be_able_to_remove_the_device()
                {
                    Assert.IsFalse(_sut.RemoveDeviceCommand.CanExecute());
                }


                [Test, Ignore]
                public void Should_not_be_able_to_test_the_device_connection()
                {
                    Assert.IsFalse(_sut.TestDeviceCommand.CanExecute());
                }
            }
        }

        [TestFixture]
        public sealed class When_device_is_in_a_processing_state : Given_a_constructed_BluetoothDevice
        {
            public override void Setup()
            {
                base.Setup();

                _bluetoothServiceMock.Setup(bs => bs.PairDevice(It.IsAny<IBluetoothDeviceInfo>()))
                    .Returns(Observable.Never<bool>());
                _bluetoothServiceMock.Setup(bs => bs.RemoveDevice(It.IsAny<IBluetoothDeviceInfo>()))
                    .Returns(Observable.Never<bool>());
            }

            private void SetToProcessing(bool isAuthenticated)
            {
                _bluetoothDeviceInfoMock.Setup(btd => btd.IsAuthenticated).Returns(isAuthenticated);
                if (isAuthenticated)
                    _sut.PairDeviceCommand.Execute();
                else
                    _sut.RemoveDeviceCommand.Execute();

                Assume.That(_sut.Status.IsProcessing, Is.True);
                Assume.That(_sut.Status.HasError, Is.False);
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_not_be_able_to_pair_the_device(bool isAuthenticated)
            {
                SetToProcessing(isAuthenticated);
                Assert.IsFalse(_sut.PairDeviceCommand.CanExecute());
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_not_be_able_to_remove_the_devicebool(bool isAuthenticated)
            {
                SetToProcessing(isAuthenticated);
                Assert.IsFalse(_sut.RemoveDeviceCommand.CanExecute());
            }
        }

        protected ITestableObservable<T> CreateSingleValueColdObservable<T>(T value)
        {
            return _testSchedulerProvider.Concurrent.CreateColdObservable(
                new Recorded<Notification<T>>(1, Notification.CreateOnNext(value)),
                new Recorded<Notification<T>>(1, Notification.CreateOnCompleted<T>())
                );
        }

        [TestFixture]
        public sealed class When_device_is_removed : Given_a_constructed_BluetoothDevice
        {
            private ITestableObservable<bool> _removeDeviceResult;

            public override void Setup()
            {
                base.Setup();

                _bluetoothDeviceInfoMock.Setup(bdi => bdi.IsAuthenticated).Returns(true);
                _removeDeviceResult = CreateSingleValueColdObservable(true);
                _bluetoothServiceMock.Setup(bs => bs.RemoveDevice(_bluetoothDeviceInfoMock.Object))
                                     .Returns(_removeDeviceResult);
            }

            [Test]
            public void Should_set_status_to_processing()
            {
                var changeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") changeCount++; };

                _sut.RemoveDeviceCommand.Execute();

                Assert.IsTrue(_sut.Status.IsProcessing);
                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_pass_deviceInfo_to_bluetoothService()
            {
                _sut.RemoveDeviceCommand.Execute();

                _bluetoothServiceMock.Verify(bs => bs.RemoveDevice(_bluetoothDeviceInfoMock.Object), Times.Once());
            }

            [Test]
            public void Should_call_bluetoothService_concurrently()
            {
                _sut.RemoveDeviceCommand.Execute();
                Assert.AreEqual(0, _removeDeviceResult.Subscriptions.Count);

                _testSchedulerProvider.Concurrent.AdvanceBy(1);//Process the subscription

                Assert.AreEqual(1, _removeDeviceResult.Subscriptions.Count);
            }

            [Test]
            public void Should_refresh_commands_on_completion()
            {
                _sut.RemoveDeviceCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var changeCount = 0;
                _sut.RemoveDeviceCommand.CanExecuteChanged += (s, e) => { changeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_set_status_to_Idle_on_completion()
            {
                _sut.RemoveDeviceCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var statusChangeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") statusChangeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.IsFalse(_sut.Status.IsProcessing);
                Assert.AreEqual(1, statusChangeCount);
            }
        }

        [TestFixture]
        public sealed class When_device_is_paired : Given_a_constructed_BluetoothDevice
        {
            private ITestableObservable<bool> _pairDeviceResult;

            public override void Setup()
            {
                base.Setup();

                _bluetoothDeviceInfoMock.Setup(bdi => bdi.IsAuthenticated).Returns(false);
                _pairDeviceResult = CreateSingleValueColdObservable(true);
                _bluetoothServiceMock.Setup(bs => bs.PairDevice(_bluetoothDeviceInfoMock.Object))
                                     .Returns(_pairDeviceResult);
            }

            [Test]
            public void Should_set_status_to_processing()
            {
                var changeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") changeCount++; };

                _sut.PairDeviceCommand.Execute();

                Assert.IsTrue(_sut.Status.IsProcessing);
                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_pass_deviceInfo_to_bluetoothService()
            {
                _sut.PairDeviceCommand.Execute();

                _bluetoothServiceMock.Verify(bs => bs.PairDevice(_bluetoothDeviceInfoMock.Object), Times.Once());
            }

            [Test]
            public void Should_call_bluetoothService_concurrently()
            {
                _sut.PairDeviceCommand.Execute();
                Assert.AreEqual(0, _pairDeviceResult.Subscriptions.Count);

                _testSchedulerProvider.Concurrent.AdvanceBy(1);//Process the subscription

                Assert.AreEqual(1, _pairDeviceResult.Subscriptions.Count);
            }

            [Test]
            public void Should_refresh_commands_on_completion()
            {
                _sut.PairDeviceCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var changeCount = 0;
                _sut.PairDeviceCommand.CanExecuteChanged += (s, e) => { changeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_set_status_to_Idle_on_completion()
            {
                _sut.PairDeviceCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var statusChangeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") statusChangeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.IsFalse(_sut.Status.IsProcessing);
                Assert.AreEqual(1, statusChangeCount);
            }
        }

        [TestFixture, Ignore]
        public sealed class When_testing_device_connection : Given_a_constructed_BluetoothDevice
        {
            public override void Setup()
            {
                base.Setup(); 
                _sut.TestDeviceCommand.Execute();
            }

            [Test]
            public void When_testing_device_connection_should_pass_DeviceInfo_to_BluetoothService()
            {
                _bluetoothServiceMock.Verify(bs => bs.TestDeviceConnection(_bluetoothDeviceInfoMock.Object), Times.Once());
            }

            [Test]
            public void Should_subscribe_to_device_connection_result_concurrently()
            {
                Assert.Inconclusive("Test not yet implemented");
            }

            [Test]
            public void Should_do_somthing_when_test_works()
            {
                Assert.Inconclusive("Test not yet implemented");
            }
            [Test]
            public void Should_do_somthing_when_test_fails()
            {
                Assert.Inconclusive("Test not yet implemented");
            }
        }
    }
}
// ReSharper restore InconsistentNaming
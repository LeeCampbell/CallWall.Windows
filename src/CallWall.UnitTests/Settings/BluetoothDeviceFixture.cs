using System;
using CallWall.Services;
using CallWall.Settings.Connectivity.Bluetooth;
using CallWall.Testing;
using InTheHand.Net.Bluetooth;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using System.Reactive.Linq;

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
            _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceType).Returns(BluetoothDeviceType.Create(DeviceClass.SmartPhone));
        }

        [TestFixture]
        public sealed class When_accessing_ToString: Given_a_constructed_BluetoothDevice
        {
            [Test]
            public void Should_return_DeviceName_in_string()
            {
                StringAssert.Contains(_expectedName, _sut.ToString());
            }

            [Test]
            public void Should_return_DeviceType_in_string(
                [AllEnumValues(typeof(DeviceClass))] DeviceClass deviceClass)
            {
                var expectedDeviceType = BluetoothDeviceType.Create(deviceClass);
                _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceType).Returns(expectedDeviceType);
                StringAssert.Contains(_sut.DeviceType.Name, _sut.ToString());
            } 
        }
        [TestFixture]
        public sealed class When_accessing_readonly_properties : Given_a_constructed_BluetoothDevice
        {
            [Test]
            public void Should_return_underlying_DeviceName()
            {
                Assert.AreEqual(_expectedName, _sut.Name);
            }

            [Test]
            public void Should_return_tranlated_DeviceType_from_factory(
                [AllEnumValues(typeof(DeviceClass))] DeviceClass deviceClass)
            {
                var expectedDeviceType = BluetoothDeviceType.Create(deviceClass);
                _bluetoothDeviceInfoMock.Setup(bt => bt.DeviceType).Returns(expectedDeviceType);

                Assert.AreEqual(expectedDeviceType, _sut.DeviceType);
            }
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


        public abstract class When_device_authentication_is_changed : Given_a_constructed_BluetoothDevice
        {
            protected abstract ITestableObservable<bool> DeviceActionResult { get; }

            protected abstract void ExecuteAuthenticationChange();

            [Test]
            public void Should_set_status_to_processing()
            {
                var changeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") changeCount++; };

                ExecuteAuthenticationChange();

                Assert.IsTrue(_sut.Status.IsProcessing);
                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_refresh_PairDeviceCommand_on_completion()
            {
                ExecuteAuthenticationChange();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var changeCount = 0;
                _sut.PairDeviceCommand.CanExecuteChanged += (s, e) => { changeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_refresh_RemoveDeviceCommand_on_completion()
            {
                ExecuteAuthenticationChange();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var changeCount = 0;
                _sut.RemoveDeviceCommand.CanExecuteChanged += (s, e) => { changeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.AreEqual(1, changeCount);
            }

            [Test]
            public void Should_set_status_to_Idle_on_completion()
            {
                ExecuteAuthenticationChange();
                _testSchedulerProvider.Concurrent.AdvanceBy(2);

                var statusChangeCount = 0;
                _sut.PropertyChanged += (s, e) => { if (e.PropertyName == "Status") statusChangeCount++; };
                _testSchedulerProvider.Async.AdvanceBy(1);

                Assert.IsFalse(_sut.Status.IsProcessing);
                Assert.AreEqual(1, statusChangeCount);
            }
            
            [Test]
            public void Should_call_bluetoothService_concurrently()
            {
                ExecuteAuthenticationChange();
                Assert.AreEqual(0, DeviceActionResult.Subscriptions.Count);

                _testSchedulerProvider.Concurrent.AdvanceBy(1); //Process the subscription

                Assert.AreEqual(1, DeviceActionResult.Subscriptions.Count);
            }

            public abstract void Should_pass_deviceInfo_to_bluetoothService();


            [TestFixture]
            public sealed class When_device_is_removed : When_device_authentication_is_changed
            {
                private ITestableObservable<bool> _removeDeviceResult;

                public override void Setup()
                {
                    base.Setup();

                    _bluetoothDeviceInfoMock.Setup(bdi => bdi.IsAuthenticated).Returns(true);
                    _removeDeviceResult = _testSchedulerProvider.Concurrent.CreateSingleValueColdObservable(true);
                    _bluetoothServiceMock.Setup(bs => bs.RemoveDevice(_bluetoothDeviceInfoMock.Object))
                        .Returns(_removeDeviceResult);
                }

                protected override void ExecuteAuthenticationChange()
                {
                    _sut.RemoveDeviceCommand.Execute();
                }

                protected override ITestableObservable<bool> DeviceActionResult
                {
                    get { return _removeDeviceResult; }
                }

                [Test]
                public override void Should_pass_deviceInfo_to_bluetoothService()
                {
                    _sut.RemoveDeviceCommand.Execute();

                    _bluetoothServiceMock.Verify(bs => bs.RemoveDevice(_bluetoothDeviceInfoMock.Object), Times.Once());
                }
            }

            [TestFixture]
            public sealed class When_device_is_paired : When_device_authentication_is_changed
            {
                private ITestableObservable<bool> _pairDeviceResult;

                public override void Setup()
                {
                    base.Setup();

                    _bluetoothDeviceInfoMock.Setup(bdi => bdi.IsAuthenticated).Returns(false);
                    _pairDeviceResult = _testSchedulerProvider.Concurrent.CreateSingleValueColdObservable(true);
                    _bluetoothServiceMock.Setup(bs => bs.PairDevice(_bluetoothDeviceInfoMock.Object))
                        .Returns(_pairDeviceResult);
                }

                protected override void ExecuteAuthenticationChange()
                {
                    _sut.PairDeviceCommand.Execute();
                }

                protected override ITestableObservable<bool> DeviceActionResult
                {
                    get { return _pairDeviceResult; }
                }

                [Test]
                public override void Should_pass_deviceInfo_to_bluetoothService()
                {
                    _sut.PairDeviceCommand.Execute();

                    _bluetoothServiceMock.Verify(bs => bs.PairDevice(_bluetoothDeviceInfoMock.Object), Times.Once());
                }
            }
        }
    }
}
// ReSharper restore InconsistentNaming
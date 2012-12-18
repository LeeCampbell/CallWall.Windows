using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CallWall.Services;
using CallWall.Settings.Bluetooth;
using InTheHand.Net.Bluetooth;
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

        //TODO Add Test for removing bluetooth device
        //TODO Add Test for pairing bluetooth device


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

            [TestCase(false)]
            [TestCase(true)]
            public void Should_not_be_able_to_pair_the_device(bool isAuthenticated)
            {
                _bluetoothDeviceInfoMock.Setup(btd => btd.IsAuthenticated).Returns(isAuthenticated);
                if(isAuthenticated)
                    _sut.PairDeviceCommand.Execute();
                else
                    _sut.RemoveDeviceCommand.Execute();

                Assume.That(_sut.Status.IsProcessing, Is.True);
                Assume.That(_sut.Status.HasError, Is.False);

                Assert.IsFalse(_sut.PairDeviceCommand.CanExecute());
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_not_be_able_to_remove_the_devicebool(bool isAuthenticated)
            {
                _bluetoothDeviceInfoMock.Setup(btd => btd.IsAuthenticated).Returns(isAuthenticated);
                if (isAuthenticated)
                    _sut.PairDeviceCommand.Execute();
                else
                    _sut.RemoveDeviceCommand.Execute();

                Assume.That(_sut.Status.IsProcessing, Is.True);
                Assume.That(_sut.Status.HasError, Is.False);

                Assert.IsFalse(_sut.RemoveDeviceCommand.CanExecute());
            }

        }
    }
}
// ReSharper restore InconsistentNaming
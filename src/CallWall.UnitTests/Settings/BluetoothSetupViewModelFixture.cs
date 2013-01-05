using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using CallWall.Services;
using CallWall.Settings.Connectivity.Bluetooth;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_BluetoothSetupViewModel
    {
        private Given_a_constructed_BluetoothSetupViewModel()
        {
        }

        private BluetoothSetupViewModel _viewModel;
        private Mock<IBluetoothService> _bluetoothServiceMock;
        private TestSchedulerProvider _testSchedulerProvider;

        [SetUp]
        public virtual void SetUp()
        {
            _bluetoothServiceMock = new Mock<IBluetoothService>();
            _bluetoothServiceMock.Setup(bs => bs.IsSupported).Returns(true);
            _bluetoothServiceMock.SetupAllProperties();
            _bluetoothServiceMock.Object.IsEnabled = true;
            _testSchedulerProvider = new TestSchedulerProvider();
            _viewModel = new BluetoothSetupViewModel(_bluetoothServiceMock.Object, _testSchedulerProvider);
        }

        protected ITestableObservable<IBluetoothDevice> CreateSequenceOfThreeOneTickApart()
        {
            var sequence = _testSchedulerProvider.Concurrent.CreateColdObservable(
                new Recorded<Notification<IBluetoothDevice>>(1, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                new Recorded<Notification<IBluetoothDevice>>(2, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                new Recorded<Notification<IBluetoothDevice>>(3, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                new Recorded<Notification<IBluetoothDevice>>(4, Notification.CreateOnCompleted<IBluetoothDevice>())
                );
            return sequence;
        }

        [TestFixture]
        public sealed class When_setting_IsEnabled_property : Given_a_constructed_BluetoothSetupViewModel
        {
            [TestCase(false)]
            [TestCase(true)]
            public void Should_set_bluetoothServiceMock_IsEnabled_to_value(bool expected)
            {
                _viewModel.IsEnabled = expected;

                Assert.AreEqual(expected, _bluetoothServiceMock.Object.IsEnabled);
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_raise_propertychanged(bool expected)
            {
                var wasRaised = false;
                _viewModel.WhenPropertyChanges(vm => vm.IsEnabled).Subscribe(_ => wasRaised = true);

                _bluetoothServiceMock.Raise(bs => bs.PropertyChanged += null, new PropertyChangedEventArgs("IsEnabled"));

                Assert.IsTrue(wasRaised);
            } 
        }

        [TestFixture]
        public sealed class When_scanning_for_devices : Given_a_constructed_BluetoothSetupViewModel
        {
            [Test]
            public void Should_set_status_to_IsProcessing_when_searching()
            {
                _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(Observable.Never<IBluetoothDevice>());
                _viewModel.ScanForDevicesCommand.Execute();
                Assert.IsTrue(_viewModel.Status.IsProcessing);
            }

            [Test]
            public void Should_clear_the_current_list_of_devices()
            {
                var sequence = CreateSequenceOfThreeOneTickApart();

                _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(sequence);
                _viewModel.ScanForDevicesCommand.Execute();
                _testSchedulerProvider.Concurrent.Start();
                _testSchedulerProvider.Async.Start();
                Assume.That(_viewModel.Devices.Count, Is.EqualTo(3));
                Assume.That(_viewModel.Status.IsProcessing, Is.False);

                _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(Observable.Empty<IBluetoothDevice>());
                _viewModel.ScanForDevicesCommand.Execute();

                Assert.AreEqual(0, _viewModel.Devices.Count);
            }

            [Test]
            public void Should_perform_search_concurrently()
            {
                var sequence = _testSchedulerProvider.Concurrent.CreateColdObservable<IBluetoothDevice>();
                _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(sequence);
                _viewModel.ScanForDevicesCommand.Execute();

                Assert.AreEqual(0, sequence.Subscriptions.Count);

                _testSchedulerProvider.Concurrent.AdvanceBy(1);

                Assert.AreEqual(1, sequence.Subscriptions.Count);
            }

            [Test]
            public void Should_not_allow_scanning_while_already_scanning()
            {
                _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(Observable.Never<IBluetoothDevice>());
                _viewModel.ScanForDevicesCommand.Execute();

                Assert.IsFalse(_viewModel.ScanForDevicesCommand.CanExecute());
            }
        }

        public abstract class When_scanning_for_devices_finishes : Given_a_constructed_BluetoothSetupViewModel
        {
            [TestFixture]
            public sealed class With_success : When_scanning_for_devices_finishes
            {
                public override void SetUp()
                {
                    base.SetUp();
                    var sequence = CreateSequenceOfThreeOneTickApart();
                    _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(sequence);
                    _viewModel.ScanForDevicesCommand.Execute();
                    _testSchedulerProvider.Concurrent.AdvanceBy(5); //1xSubscribe, 3xOnNext, 1xOnCompleted
                    _testSchedulerProvider.Async.AdvanceBy(5);
                }

                [Test]
                public void Should_update_Devices_with_search_results_on_UIThread()
                {
                    Assert.AreEqual(3, _viewModel.Devices.Count);
                }
            }

            [TestFixture]
            public sealed class With_failure : When_scanning_for_devices_finishes
            {
                private const string _expectedMessage = "The ship went down!";

                public override void SetUp()
                {
                    base.SetUp();
                    _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(
                        Observable.Throw<IBluetoothDevice>(new Exception(_expectedMessage)));
                    _viewModel.ScanForDevicesCommand.Execute();
                    _testSchedulerProvider.Concurrent.AdvanceBy(1);
                    _testSchedulerProvider.Async.AdvanceBy(1);
                }

                [Test]
                public void Should_set_status_to_errored()
                {
                    Assert.IsTrue(_viewModel.Status.HasError);
                    Assert.AreEqual(_expectedMessage, _viewModel.Status.ErrorMessage);
                }
            }

            [TestFixture]
            public sealed class With_zero_results : When_scanning_for_devices_finishes
            {
                private const string _expectedMessage = "No devices were found. Ensure that Bluetooth is enabled on both your phone and your computer, and that the device is in range. You may also have to enable Bluetooth discovery on your phone.";

                public override void SetUp()
                {
                    base.SetUp();
                    _bluetoothServiceMock.Setup(bs => bs.ScanForDevices()).Returns(
                        Observable.Empty<IBluetoothDevice>());
                    _viewModel.ScanForDevicesCommand.Execute();
                    _testSchedulerProvider.Concurrent.AdvanceBy(1);
                    _testSchedulerProvider.Async.AdvanceBy(1);
                }

                [Test]
                public void Should_set_status_to_errored()
                {
                    Assert.IsTrue(_viewModel.Status.HasError);
                }

                [Test]
                public void Should_set_status_error_message()
                {
                    Assert.AreEqual(_expectedMessage, _viewModel.Status.ErrorMessage);
                }
            }

            [Test]
            public void Should_set_status_IsProcessing_to_false()
            {
                Assert.IsFalse(_viewModel.Status.IsProcessing);
            }

            [Test]
            public void Should_allow_scanning_again()
            {
                Assert.IsTrue(_viewModel.ScanForDevicesCommand.CanExecute());
            }
        }
    }
}
// ReSharper restore InconsistentNaming
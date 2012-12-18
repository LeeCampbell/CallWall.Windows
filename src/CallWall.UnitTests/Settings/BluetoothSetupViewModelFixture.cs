using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CallWall.Services;
using CallWall.Settings.Bluetooth;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_BluetoothSetupViewModel
    {
        private Given_a_constructed_BluetoothSetupViewModel()
        { }

        private BluetoothSetupViewModel _viewModel;
        private Mock<IBluetoothService> _bluetoothServiceMock;
        private TestSchedulerProvider _testSchedulerProvider;

        [SetUp]
        public virtual void SetUp()
        {
            _bluetoothServiceMock = new Mock<IBluetoothService>();
            _testSchedulerProvider = new TestSchedulerProvider();
            _viewModel = new BluetoothSetupViewModel(_bluetoothServiceMock.Object, _testSchedulerProvider);
        }

        [TestFixture]
        public sealed class When_scanning_for_devices : Given_a_constructed_BluetoothSetupViewModel
        {
            [Test]
            public void Should_set_status_to_IsProcessing_when_searching()
            {
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Never<IBluetoothDevice>());
                _viewModel.SearchForDevicesCommand.Execute();
                Assert.IsTrue(_viewModel.Status.IsProcessing);
            }

            [Test]
            public void Should_set_status_to_idle_when_search_is_complete()
            {
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Empty<IBluetoothDevice>());
                _viewModel.SearchForDevicesCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(1);
                _testSchedulerProvider.Async.AdvanceBy(1);
                Assert.IsFalse(_viewModel.Status.IsProcessing);
            }

            [Test]
            public void Should_perform_search_concurrently()
            {
                var sequence = _testSchedulerProvider.Concurrent.CreateColdObservable<IBluetoothDevice>();
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(sequence);
                _viewModel.SearchForDevicesCommand.Execute();

                Assert.AreEqual(0, sequence.Subscriptions.Count);

                _testSchedulerProvider.Concurrent.AdvanceBy(1);

                Assert.AreEqual(1, sequence.Subscriptions.Count);
            }

            [Test]
            public void Should_update_Devices_with_search_results_on_UIThread()
            {
                var sequence = CreateSequenceOfThreeOneTickApart();

                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(sequence);
                _viewModel.SearchForDevicesCommand.Execute();
                _testSchedulerProvider.Concurrent.AdvanceBy(5);//1xSubscribe, 3xOnNext, 1xOnCompleted
                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(i, _viewModel.Devices.Count);
                    _testSchedulerProvider.Async.AdvanceBy(1);    
                }
                Assert.AreEqual(3, _viewModel.Devices.Count);
                Assert.IsTrue(_viewModel.Status.IsProcessing);
                _testSchedulerProvider.Async.AdvanceBy(1);
                Assert.IsFalse(_viewModel.Status.IsProcessing);
            }

            [Test]
            public void Should_set_status_to_errored_if_search_fails()
            {
                var expectedMessage = "The ship went down!";
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Throw<IBluetoothDevice>(new Exception(expectedMessage)));

                _viewModel.SearchForDevicesCommand.Execute();
                _testSchedulerProvider.Concurrent.Start();
                _testSchedulerProvider.Async.Start();

                Assert.IsTrue(_viewModel.Status.HasError);
                Assert.AreEqual(expectedMessage, _viewModel.Status.ErrorMessage);
            }

            [Test]
            public void Should_not_allow_scanning_while_already_scanning()
            {
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Never<IBluetoothDevice>());
                _viewModel.SearchForDevicesCommand.Execute();

                Assert.IsFalse(_viewModel.SearchForDevicesCommand.CanExecute());
            }

            [Test]
            public void Should_allow_scanning_when_scan_complete()
            {
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Empty<IBluetoothDevice>());
                _viewModel.SearchForDevicesCommand.Execute();

                _testSchedulerProvider.Concurrent.AdvanceBy(1);
                _testSchedulerProvider.Async.AdvanceBy(1);
                Assert.IsTrue(_viewModel.SearchForDevicesCommand.CanExecute());
            }

            [Test]
            public void Should_allow_scanning_when_scan_errors()
            {
                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Throw<IBluetoothDevice>(new Exception()));
                _viewModel.SearchForDevicesCommand.Execute();

                _testSchedulerProvider.Concurrent.AdvanceBy(1);
                _testSchedulerProvider.Async.AdvanceBy(1);
                Assert.IsTrue(_viewModel.SearchForDevicesCommand.CanExecute());
            }

            [Test]
            public void Should_clear_the_current_list_of_devices()
            {
                var sequence = CreateSequenceOfThreeOneTickApart();

                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(sequence);
                _viewModel.SearchForDevicesCommand.Execute();
                _testSchedulerProvider.Concurrent.Start();
                _testSchedulerProvider.Async.Start();
                Assume.That(_viewModel.Devices.Count, Is.EqualTo(3));
                Assume.That(_viewModel.Status.IsProcessing, Is.False);

                _bluetoothServiceMock.Setup(bs => bs.SearchForDevices()).Returns(Observable.Empty<IBluetoothDevice>());
                _viewModel.SearchForDevicesCommand.Execute();

                Assert.AreEqual(0, _viewModel.Devices.Count);
            }

            private ITestableObservable<IBluetoothDevice> CreateSequenceOfThreeOneTickApart()
            {
                var sequence = _testSchedulerProvider.Concurrent.CreateColdObservable(
                    new Recorded<Notification<IBluetoothDevice>>(1, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                    new Recorded<Notification<IBluetoothDevice>>(2, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                    new Recorded<Notification<IBluetoothDevice>>(3, Notification.CreateOnNext(new Mock<IBluetoothDevice>().Object)),
                    new Recorded<Notification<IBluetoothDevice>>(4, Notification.CreateOnCompleted<IBluetoothDevice>())
                    );
                return sequence;
            }
        }

    }
}
// ReSharper restore InconsistentNaming
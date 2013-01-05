using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallWall.Settings.Connectivity.Bluetooth;
using Moq;
using NUnit.Framework;

namespace CallWall.UnitTests.Settings
{
    [TestFixture]
    public sealed class Given_a_constructed_BluetoothConnectionConfiguration
    {
        private Mock<IBluetoothSetupView> _viewMock;
        private BluetoothConnectionConfiguration _sut;
        private Mock<IBluetoothSetupViewModel> _viewModelMock;

        [SetUp]
        public void SetUp()
        {
            _viewModelMock = new Mock<IBluetoothSetupViewModel>();
            _viewMock = new Mock<IBluetoothSetupView>();
            _viewMock.Setup(v => v.ViewModel).Returns(_viewModelMock.Object);
            _sut = new BluetoothConnectionConfiguration(_viewMock.Object);
        }

        [Test]
        public void Should_return_Bluetooth_from_Name()
        {
            Assert.AreEqual("Bluetooth", _sut.Name);
        }

        [Test]
        public void Should_return_Bluetooth_image_from_Image()
        {
            var expected = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth_72x72.png");
            Assert.AreEqual(expected, _sut.Image);
        }

        [Test]
        public void Should_return_injected_IBluetoothSetupView_from_View()
        {
            Assert.AreSame(_viewMock.Object, _sut.View);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Should_return_IsEnabled_value_from_view(bool isEnabled)
        {
            _viewModelMock.SetupGet(vm => vm.IsEnabled).Returns(isEnabled);

            Assert.AreEqual(isEnabled, _sut.IsEnabled);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Should_set_IsEnabled_value_on_view(bool isEnabled)
        {
            _sut.IsEnabled = isEnabled;

            _viewModelMock.VerifySet(vm => vm.IsEnabled = isEnabled); ;
        }

        [Test]
        public void Should_raise_propertyChanged_when_View_IsEnabled_changes()
        {
            var wasRaised = false;
            _sut.WhenPropertyChanges(s => s.IsEnabled).Subscribe(_ => wasRaised = true);

            _viewModelMock.Raise(vm => vm.PropertyChanged += null, new PropertyChangedEventArgs("IsEnabled"));

            Assert.IsTrue(wasRaised);
        }
    }
}

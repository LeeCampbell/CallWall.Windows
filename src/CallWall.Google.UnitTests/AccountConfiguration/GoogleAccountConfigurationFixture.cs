using System;
using System.ComponentModel;
using CallWall.Google.AccountConfiguration;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.AccountConfiguration
{
    [TestFixture]
    public sealed class GoogleAccountConfigurationFixture
    {
        private GoogleAccountConfiguration _googleAccountConfiguration;
        private IGoogleAccountSetupView _googleAccountSetupView;
        private GoogleAccountSetupViewModel _googleAccountSetupViewModel;
        private Mock<IGoogleAccountSetup> _accountSetupMock;

        [SetUp]
        public void SetUp()
        {
            var viewMock = new Mock<IGoogleAccountSetupView>();
            _accountSetupMock = new Mock<IGoogleAccountSetup>();
            _accountSetupMock.SetupAllProperties();
            _googleAccountSetupViewModel = new GoogleAccountSetupViewModel(_accountSetupMock.Object);
            viewMock.Setup(v => v.ViewModel).Returns(_googleAccountSetupViewModel);
            _googleAccountSetupView = viewMock.Object;
            _googleAccountConfiguration = new GoogleAccountConfiguration(_googleAccountSetupView);
        }

        [Test]
        public void Should_return_Google_as_Name()
        {
            Assert.AreEqual("Google", _googleAccountConfiguration.Name);
        }

        [Test]
        public void Should_return_google_icon_as_Image()
        {
            Assert.AreEqual(new Uri("pack://application:,,,/CallWall.Google;component/Images/Google_64x64.png"), _googleAccountConfiguration.Image);
        }

        [Test]
        public void Should_return_Injected_view_as_View()
        {
            Assert.AreEqual(_googleAccountSetupView, _googleAccountConfiguration.View);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Should_set_ViewModel_IsEnabled_when_Setting_IsEnabled(bool expected)
        {
            _googleAccountConfiguration.IsEnabled = expected;
            Assert.AreEqual(expected, _googleAccountConfiguration.IsEnabled);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Should_get_IsEnabled_from_ViewModel_IsEnabled(bool expected)
        {
            _googleAccountSetupViewModel.IsEnabled = expected;
            Assert.AreEqual(expected, _accountSetupMock.Object.IsEnabled);
        }

        [Test]
        public void Should_raise_propertyChanged_when_ViewModel_raises_propertyChanged_for_IsEnabled()
        {
            var propertyNameRaised = string.Empty;
            _googleAccountConfiguration.PropertyChanged += (s, e) => { propertyNameRaised = e.PropertyName; };
            _accountSetupMock.Raise(a=>a.PropertyChanged+=null, new PropertyChangedEventArgs("IsEnabled"));
            
            Assert.AreEqual("IsEnabled", propertyNameRaised);
        }
        
    }
}
// ReSharper restore InconsistentNaming
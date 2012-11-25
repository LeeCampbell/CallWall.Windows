using CallWall.Settings;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Providers;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_SettingsModule
    {
        #region Setup

        private SettingsModule _settingsModule;
        private Mock<IUnityContainer> _containerMock;

        private Given_a_constructed_SettingsModule()
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _settingsModule = new SettingsModule(_containerMock.Object);
        }

        #endregion

        [TestFixture]
        public class When_Initialized : Given_a_constructed_SettingsModule
        {
            public override void SetUp()
            {
                base.SetUp();
                _settingsModule.Initialize();
            }

            [Test]
            public void Should_register_ConnectivitySettingsView_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IConnectivitySettingsView), typeof(ConnectivitySettingsView), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_ConnectivitySettingsViewModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IConnectivitySettingsViewModel), typeof(ConnectivitySettingsViewModel), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_ConnectivitySettingsModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IConnectivitySettingsModel), typeof(ConnectivitySettingsModel), (string)null, It.IsAny<TransientLifetimeManager>()));
            }

            [Test]
            public void Should_register_ProviderSettingsView_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IProviderSettingsView), typeof(ProviderSettingsView), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_ProviderSettingsViewModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IProviderSettingsViewModel), typeof(ProviderSettingsViewModel), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_ProviderSettingsModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IProviderSettingsModel), typeof(ProviderSettingsModel), (string)null, It.IsAny<TransientLifetimeManager>()));
            }

            [Test]
            public void Should_register_for_some_event_to_show_the_settings_view()
            {
                Assert.Inconclusive("Test not yet implemented");
            }
        }
    }
}
// ReSharper restore InconsistentNaming
using CallWall.Settings;
using CallWall.Settings.Accounts;
using CallWall.Settings.Connectivity;
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
            public void Should_register_AccountSettingsView_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IAccountSettingsView), typeof(AccountSettingsView), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_AccountSettingsViewModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IAccountSettingsViewModel), typeof(AccountSettingsViewModel), (string)null, It.IsAny<TransientLifetimeManager>()));
            }
            [Test]
            public void Should_register_AccountSettingsModel_to_container()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IAccountSettingsModel), typeof(AccountSettingsModel), (string)null, It.IsAny<TransientLifetimeManager>()));
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
using CallWall.Contract;
using CallWall.Settings;
using CallWall.Settings.Accounts;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Demonstration;
using Moq;
using NUnit.Framework;

// ReSharper disable ReplaceWithSingleCallToSingle
// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_SettingsModule
    {
        #region Setup

        private SettingsModule _settingsModule;
        private Mock<ITypeRegistry> _typeRegistryMock;

        private Given_a_constructed_SettingsModule()
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            _typeRegistryMock = new Mock<ITypeRegistry>();
            _settingsModule = new SettingsModule(_typeRegistryMock.Object);
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
            public void Should_register_ConnectivitySettingsView_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IConnectivitySettingsView, ConnectivitySettingsView>(), Times.Once());
            }
            [Test]
            public void Should_register_ConnectivitySettingsViewModel_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IConnectivitySettingsViewModel, ConnectivitySettingsViewModel>(), Times.Once());
            }
            [Test]
            public void Should_register_ConnectivitySettingsModel_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IConnectivitySettingsModel, ConnectivitySettingsModel>(), Times.Once());
            }

            [Test]
            public void Should_register_AccountSettingsView_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IAccountSettingsView, AccountSettingsView>(), Times.Once());
            }
            [Test]
            public void Should_register_AccountSettingsViewModel_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IAccountSettingsViewModel, AccountSettingsViewModel>(), Times.Once());
            }
            [Test]
            public void Should_register_AccountSettingsModel_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IAccountSettingsModel, AccountSettingsModel>(), Times.Once());
            }

            [Test]
            public void Should_register_DemoView_as_transient()
            {
                _typeRegistryMock.Verify(c => c.RegisterTypeAsTransient<IDemoView, DemoView>(), Times.Once());
            }

            [Test]
            public void Should_register_DemoActivatedIdentityListener_to_container_as_IDemoProfileActivator_and_IProfileActivator()
            {
                _typeRegistryMock.Verify(c => c.RegisterCompositeAsSingleton<IProfileActivator, IDemoProfileActivator, DemoActivatedIdentityListener>(), Times.Once());
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
// ReSharper restore ReplaceWithSingleCallToSingle
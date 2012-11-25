using CallWall.Settings.Connectivity;
using CallWall.Settings.Providers;
using CallWall.Welcome;
using Microsoft.Practices.Prism.Regions;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Welcome
{
    public abstract class Given_a_started_WelcomeController
    {
        #region Setup
        private WelcomeController _welcomeController;
        private RegionManagerStub _regionManagerStub;
        private Mock<IWelcomeView> _welcomeViewMock;
        private Mock<IRegion> _modalRegion;
        private Mock<IProviderSettingsViewModel> _providerSettingsVMMock;
        private Mock<IConnectivitySettingsViewModel> _connectivitySettingsVMMock;
        private Mock<IProviderSettingsView> _providerSettingsViewMock;
        private Mock<IConnectivitySettingsView> _connectivitySettingsViewMock;
        private Mock<IRegion> _connectivitySettingsRegion;
        private Mock<IRegion> _providersSettingsRegion;

        private Given_a_started_WelcomeController()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _regionManagerStub = new RegionManagerStub();
            _modalRegion = _regionManagerStub.CreateAndAddMock(RegionNames.Modal);
            _connectivitySettingsRegion = _regionManagerStub.CreateAndAddMock(ShellRegionNames.ConnectivitySettingsRegion);
            _providersSettingsRegion = _regionManagerStub.CreateAndAddMock(ShellRegionNames.ProvidersSettingsRegion);

            _welcomeViewMock = new Mock<IWelcomeView>();

            _connectivitySettingsVMMock = new Mock<IConnectivitySettingsViewModel>();
            _connectivitySettingsViewMock = new Mock<IConnectivitySettingsView>();
            _connectivitySettingsViewMock.Setup(v => v.ViewModel).Returns(_connectivitySettingsVMMock.Object);

            _providerSettingsVMMock = new Mock<IProviderSettingsViewModel>();
            _providerSettingsViewMock = new Mock<IProviderSettingsView>();
            _providerSettingsViewMock.Setup(v => v.ViewModel).Returns(_providerSettingsVMMock.Object);

            _welcomeController = new WelcomeController(_regionManagerStub,
                _welcomeViewMock.Object,
                _connectivitySettingsViewMock.Object,
                _providerSettingsViewMock.Object);
        }
        #endregion

        public abstract class When_user_has_not_been_set_up : Given_a_started_WelcomeController
        {
            [TestFixture]
            public sealed class With_Connectivity_requiring_setup : When_user_has_not_been_set_up
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _connectivitySettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(true);
                    _welcomeController.Start();
                }
            }

            [TestFixture]
            public sealed class With_Providers_requiring_setup : When_user_has_not_been_set_up
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _providerSettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(true);
                    _welcomeController.Start();
                }
            }

            [Test]
            public void Should_show_welcome_view_in_modal_region()
            {
                _modalRegion.Verify(r => r.Add(_welcomeViewMock.Object), Times.Once());
            }

            [Test]
            public void Should_show_Connectivity_settings_view_in_inactive_Connectivity_region()
            {
                _connectivitySettingsRegion.Verify(r => r.Add(_connectivitySettingsViewMock.Object), Times.Once());
            }

            [Test]
            public void Should_show_Provider_settings_view_in_inactive_Provider_region()
            {
                _providersSettingsRegion.Verify(r => r.Add(_providerSettingsViewMock.Object), Times.Once());
            }
        }

        [TestFixture]
        public class When_user_has_been_set_up : Given_a_started_WelcomeController
        {
            public override void SetUp()
            {
                base.SetUp();
                _connectivitySettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(false);
                _providerSettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(false);
            }

            [Test]
            public void Should_not_show_welcome_view_in_modal_region()
            {
                _modalRegion.Verify(r => r.Add(_welcomeViewMock.Object), Times.Never());
            }
        }
    }

    public abstract class Given_the_WelcomeModule_is_showing_the_WelcomeView
    {
        private Given_the_WelcomeModule_is_showing_the_WelcomeView()
        { }



        [TestFixture]
        public class When_the_Connectivity_settings_view_is_closed : Given_the_WelcomeModule_is_showing_the_WelcomeView
        {
            [Test]
            public void Should_activate_the_Provider_settings_view()
            {
                Assert.Inconclusive();
            }
        }

        [TestFixture]
        public class When_the_Provider_settings_view_is_closed
        {
            [Test]
            public void Should_activate_the_SampleDisplay_view()
            {
                Assert.Inconclusive();
            }
        }

        [TestFixture]
        public class When_the_SampleDisplay_view_is_closed
        {
            [Test]
            public void Should_close_WelcomeView()
            {
                Assert.Inconclusive();
            }

            [Test]
            public void Should_dispose_child_RegionManager()
            {
                Assert.Inconclusive();
            }
        }
    }
}

using System;
using CallWall.Settings.Accounts;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Demonstration;
using CallWall.Welcome;
using Microsoft.Practices.Prism.Regions;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Welcome
{
    public abstract class Given_a_constructed_WelcomeController
    {
        #region Setup
        private WelcomeController _welcomeController;
        private RegionManagerStub _regionManagerStub;
        private Mock<IWelcomeView> _welcomeViewMock;
        private Mock<IRegion> _modalRegion;
        private Mock<IAccountSettingsViewModel> _accountSettingsVMMock;
        private Mock<IConnectivitySettingsViewModel> _connectivitySettingsVMMock;
        private Mock<IAccountSettingsView> _accountSettingsViewMock;
        private Mock<IConnectivitySettingsView> _connectivitySettingsViewMock;
        private Mock<IRegion> _welcomeSettingsRegion;
        private Mock<IWelcomeStep1View> _welcomeStep1ViewMock;
        private Mock<IDemoView> _demoViewMock;

        private Given_a_constructed_WelcomeController()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _regionManagerStub = new RegionManagerStub();
            _modalRegion = _regionManagerStub.CreateAndAddMock(RegionNames.WindowRegion);
            _welcomeSettingsRegion = _regionManagerStub.CreateAndAddMock(ShellRegionNames.WelcomeSettingsRegion);

            _welcomeViewMock = new Mock<IWelcomeView>();
            var vm = new WelcomeViewModel();
            _welcomeViewMock.SetupGet(v => v.ViewModel).Returns(vm);

            _connectivitySettingsVMMock = new Mock<IConnectivitySettingsViewModel>();
            _connectivitySettingsViewMock = new Mock<IConnectivitySettingsView>();
            _connectivitySettingsViewMock.Setup(v => v.ViewModel).Returns(_connectivitySettingsVMMock.Object);

            _accountSettingsVMMock = new Mock<IAccountSettingsViewModel>();
            _accountSettingsViewMock = new Mock<IAccountSettingsView>();
            _accountSettingsViewMock.Setup(v => v.ViewModel).Returns(_accountSettingsVMMock.Object);

            _welcomeStep1ViewMock = new Mock<IWelcomeStep1View>();
            _demoViewMock = new Mock<IDemoView>();

            _welcomeController = new WelcomeController(_regionManagerStub,
                _welcomeViewMock.Object,
                _welcomeStep1ViewMock.Object,
                _connectivitySettingsViewMock.Object,
                _accountSettingsViewMock.Object, 
                _demoViewMock.Object);
        }
        #endregion

        public abstract class When_user_has_not_been_set_up : Given_a_constructed_WelcomeController
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
            public sealed class With_Accounts_requiring_setup : When_user_has_not_been_set_up
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _accountSettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(true);
                    _welcomeController.Start();
                }
            }

            [Test]
            public void Should_show_welcome_view_in_modal_region()
            {
                _modalRegion.Verify(r => r.Add(_welcomeViewMock.Object), Times.Once());
            }

            [Test]
            public void Should_show_Connectivity_settings_view_in_inactive_WelcomeSettingsRegion_region()
            {
                _welcomeSettingsRegion.Verify(r => r.Add(_connectivitySettingsViewMock.Object), Times.Once());
            }

            [Test]
            public void Should_show_Provider_settings_view_in_inactive_WelcomeSettingsRegion_region()
            {
                _welcomeSettingsRegion.Verify(r => r.Add(_accountSettingsViewMock.Object), Times.Once());
            }
        }

        [TestFixture]
        public class When_user_has_been_set_up : Given_a_constructed_WelcomeController
        {
            public override void SetUp()
            {
                base.SetUp();
                _connectivitySettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(false);
                _accountSettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(false);
            }

            [Test]
            public void Should_not_show_welcome_view_in_modal_region()
            {
                _modalRegion.Verify(r => r.Add(_welcomeViewMock.Object), Times.Never());
            }
        }

        public abstract class With_the_WelcomeModule_is_showing_the_WelcomeView : Given_a_constructed_WelcomeController
        {
            #region Setup
            
            private With_the_WelcomeModule_is_showing_the_WelcomeView()
            { }

            public override void SetUp()
            {
                base.SetUp();

                _connectivitySettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(true);
                _accountSettingsVMMock.SetupGet(vm => vm.RequiresSetup).Returns(true);
                _welcomeController.Start();
            }
            #endregion

            [TestFixture]
            public class When_the_WelcomeStep1_view_is_closed : With_the_WelcomeModule_is_showing_the_WelcomeView
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _welcomeStep1ViewMock.Raise(vm => vm.NextView += (sender, args) => { }, EventArgs.Empty);
                }

                [Test]
                public void Should_activate_the_Connectivity_settings_view()
                {
                    _welcomeSettingsRegion.Verify(r => r.Activate(_connectivitySettingsViewMock.Object), Times.Once());
                }
            }

            [TestFixture]
            public class When_the_Connectivity_settings_view_is_closed : With_the_WelcomeModule_is_showing_the_WelcomeView
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _connectivitySettingsVMMock.Raise(vm => vm.Closed += (sender, args) => { }, EventArgs.Empty);
                }

                [Test]
                public void Should_activate_the_Provider_settings_view()
                {
                    _welcomeSettingsRegion.Verify(r => r.Activate(_accountSettingsViewMock.Object), Times.Once());
                }
            }
            [TestFixture]
            public class When_the_Provider_settings_view_is_closed : With_the_WelcomeModule_is_showing_the_WelcomeView
            {
                public override void SetUp()
                {
                    base.SetUp();
                    _accountSettingsVMMock.Raise(vm => vm.Closed += (sender, args) => { }, EventArgs.Empty);
                }

                [Test]
                public void Should_activate_the_DemoDisplay_view()
                {
                    _welcomeSettingsRegion.Verify(r => r.Activate(_demoViewMock.Object), Times.Once());
                }
            }

            [TestFixture]
            public class When_the_SampleDisplay_view_is_closed : With_the_WelcomeModule_is_showing_the_WelcomeView
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
}

using System;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Providers;
using CallWall.Welcome;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Welcome
{
    public abstract class Given_a_started_WelcomeController
    {
        private WelcomeController _welcomeController;
        private Mock<IRegionManager> _regionManagerMock;
        private Mock<IWelcomeView> _welcomeViewMock;

        private Given_a_started_WelcomeController()
        { }

        [SetUp]
        public virtual void SetUp()
        {
            _regionManagerMock = new Mock<IRegionManager>();
            _welcomeViewMock = new Mock<IWelcomeView>();
            _welcomeController = new WelcomeController(_regionManagerMock.Object,
                _welcomeViewMock.Object,
                new Mock<IConnectivitySettingsView>().Object,
                new Mock<IProviderSettingsView>().Object);

            _welcomeController.Start();
        }

        [TestFixture]
        public class When_user_has_not_been_set_up : Given_a_started_WelcomeController
        {

            public override void SetUp()
            {
                base.SetUp();
                //TODO: set up so that the connectivity settings are all disabled and so are the provider settings.
            }

            [Test]
            public void Should_show_welcome_view_in_modal_region()
            {
                Assert.Inconclusive();
                //_regionManagerMock.Verify(rm => rm.AddToRegion("ModalRegion", _welcomeViewMock), Times.Once());
            }

            [Test]
            public void Should_show_Connectivity_settings_view_in_inactive_Connectivity_sub_region()
            {
                Assert.Inconclusive();
            }

            [Test]
            public void Should_show_Provider_settings_view_in_inactive_Provider_sub_region()
            {
                Assert.Inconclusive();
            }
        }

        [TestFixture]
        public class When_user_has_been_set_up
        {
            [Test]
            public void Should_not_show_welcome_view_in_modal_region()
            {
                Assert.Inconclusive();
            }
        }
    }

    public abstract class Given_the_WelcomeModule_is_showing_the_WelcomeView
    {
        private Given_the_WelcomeModule_is_showing_the_WelcomeView()
        { }

        [TestFixture]
        public class When_the_Connectivity_settings_view_is_closed
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

using System.Linq;
using CallWall.Settings;
using CallWall.Settings.Accounts;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Demonstration;
using Microsoft.Practices.Unity;
using NUnit.Framework;

// ReSharper disable ReplaceWithSingleCallToSingle
// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    public abstract class Given_a_constructed_SettingsModule
    {
        #region Setup

        private SettingsModule _settingsModule;
        private StubUnityContainer _container;

        private Given_a_constructed_SettingsModule()
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            _container = new StubUnityContainer();
            _settingsModule = new SettingsModule(_container);
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
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IConnectivitySettingsView))
                    .Where(rt => rt.To == typeof(ConnectivitySettingsView))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_ConnectivitySettingsViewModel_to_container()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IConnectivitySettingsViewModel))
                    .Where(rt => rt.To == typeof(ConnectivitySettingsViewModel))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_ConnectivitySettingsModel_to_container()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IConnectivitySettingsModel))
                    .Where(rt => rt.To == typeof(ConnectivitySettingsModel))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }

            [Test]
            public void Should_register_AccountSettingsView_to_container()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IAccountSettingsView))
                    .Where(rt => rt.To == typeof(AccountSettingsView))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_AccountSettingsViewModel_to_container()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IAccountSettingsViewModel))
                    .Where(rt => rt.To == typeof(AccountSettingsViewModel))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_AccountSettingsModel_to_container()
            {

                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IAccountSettingsModel))
                    .Where(rt => rt.To == typeof(AccountSettingsModel))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();

            }

            [Test]
            public void Should_register_DemoView_to_container()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IDemoView))
                    .Where(rt => rt.To == typeof(DemoView))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is TransientLifetimeManager)
                    .Single();
            }

            /*
                This is a strategy for registering a singleton but as two separate types. This maintains lazy instantiation.
             
             */
            [Test]
            public void Should_register_DemoActivatedIdentityListener_to_container_as_IDemoActivatedIdentityListener()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IDemoActivatedIdentityListener))
                    .Where(rt => rt.To == typeof(DemoActivatedIdentityListener))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is ContainerControlledLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_DemoActivatedIdentityListener_to_container_as_named_IActivatedIdentityListener()
            {
                _container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IActivatedIdentityListener))
                    .Where(rt => rt.To == typeof(DemoActivatedIdentityListener))
                    .Where(rt => rt.Name == "DemoActivatedIdentityListener")
                    .Where(rt => rt.LifetimeManager is ExternallyControlledLifetimeManager) //} HACK: A poor way of checking if the instance is registered
                    .Where(rt => rt.InjectionMembers.Single() is InjectionFactory)          //} to actually redirect to the registered IDemoActivatedIdentityListener instance/type.
                    .Single();
            }

            //[Test]
            //public void Should_register_same_instance_of_DemoActivatedIdentityListener_to_as_IActivatedIdentityListener_and_IDemoActivatedIdentityListener()
            //{

            //    var activatedInstance = _container.RegisteredInstances
            //        .Where(rt => rt.Type == typeof(IActivatedIdentityListener))
            //        .Where(rt => rt.Instance is DemoActivatedIdentityListener)
            //        .Where(rt => rt.Name == null)
            //        .Where(rt => rt.LifetimeManager is ContainerControlledLifetimeManager)
            //        .Single();
            //    var demoInstance = _container.RegisteredInstances
            //        .Where(rt => rt.Type == typeof(IActivatedIdentityListener))
            //        .Where(rt => rt.Instance is DemoActivatedIdentityListener)
            //        .Where(rt => rt.Name == null)
            //        .Where(rt => rt.LifetimeManager is ContainerControlledLifetimeManager)
            //        .Single();
            //    Assert.AreSame(activatedInstance.Instance, demoInstance.Instance);
            //}

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
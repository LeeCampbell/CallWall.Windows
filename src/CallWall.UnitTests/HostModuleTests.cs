using System.Linq;
using CallWall.Activators;
using CallWall.ProfileDashboard;
using CallWall.Services;
using CallWall.Testing;
using CallWall.Web;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests
{
    public abstract class Given_a_constructed_HostModule
    {
        #region Setup

        private Given_a_constructed_HostModule()
        {
        }

        private HostModule _hostModule;
        private Mock<IUnityContainer> _containerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _containerMock = new Mock<IUnityContainer>();
            _hostModule = new HostModule(_containerMock.Object);
        }

        #endregion

        [TestFixture]
        public sealed class When_Initialised : Given_a_constructed_HostModule
        {
            public override void SetUp()
            {
                base.SetUp();
                _hostModule.Initialize();
            }

            [Test]
            public void Should_register_SchedulerProvider_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(ISchedulerProvider), typeof(SchedulerProvider), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
            [Test]
            public void Should_register_PersonalizationSettings_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IPersonalizationSettings), typeof(PersonalizationSettings), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
            [Test]
            public void Should_register_HttpClient_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IHttpClient), typeof(HttpClient), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
            [Test]
            public void Should_register_BluetoothService_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IBluetoothService), typeof(BluetoothService), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }
            [Test]
            public void Should_register_ProfileActivatorAggregator_instance()
            {
                _containerMock.Verify(c => c.RegisterType(typeof(IProfileActivatorAggregator), typeof(ProfileActivatorAggregator), (string)null, It.IsAny<ContainerControlledLifetimeManager>()), Times.Once());
            }

            /*
                This is a strategy for registering a singleton but as two separate types. This maintains lazy instantiation.
            */
            [Test]
            public void Should_register_BluetoothProfileActivator_to_container_as_IBluetoothProfileActivator()
            {
                var container = new StubUnityContainer();
                _hostModule = new HostModule(container);
                _hostModule.Initialize();
                container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IBluetoothProfileActivator))
                    .Where(rt => rt.To == typeof(BluetoothProfileActivator))
                    .Where(rt => rt.Name == null)
                    .Where(rt => rt.LifetimeManager is ContainerControlledLifetimeManager)
                    .Single();
            }
            [Test]
            public void Should_register_DemoActivatedIdentityListener_to_container_as_named_IActivatedIdentityListener()
            {
                var container = new StubUnityContainer();
                _hostModule = new HostModule(container);
                _hostModule.Initialize();
                container.RegisteredTypes
                    .Where(rt => rt.From == typeof(IProfileActivator))
                    .Where(rt => rt.To == typeof(BluetoothProfileActivator))
                    .Where(rt => rt.Name == "BluetoothProfileActivator")
                    .Where(rt => rt.LifetimeManager is ExternallyControlledLifetimeManager) //} HACK: A poor way of checking if the instance is registered
                    .Where(rt => rt.InjectionMembers.Single() is InjectionFactory)          //} to actually redirect to the registered IDemoActivatedIdentityListener instance/type.
                    .Single();
            }

        }
    }
}
// ReSharper restore InconsistentNaming
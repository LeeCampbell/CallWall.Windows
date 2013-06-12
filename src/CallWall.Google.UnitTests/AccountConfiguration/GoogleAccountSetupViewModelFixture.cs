using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using CallWall.Google.AccountConfiguration;
using CallWall.Google.Authorization;
using CallWall.Testing;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.AccountConfiguration
{
    [TestFixture]
    public sealed class GoogleAccountSetupViewModelFixture
    {
        private const string _googleIsEnabledSettingsKey = "CallWall.Google.AccountConfiguration.GoogleAccountSetup.IsEnabled";
        private GoogleAccountSetupViewModel _sut;
        private Mock<IPersonalizationSettings> _settingsMock;
        private Mock<IGoogleAuthorization> _authorizationMock;
        private TestSchedulerProvider _testSchedulerProvider;

        [SetUp]
        public void SetUp()
        {
            _settingsMock = new Mock<IPersonalizationSettings>();
            _authorizationMock = new Mock<IGoogleAuthorization>();
            _authorizationMock.SetupGet(a => a.Status).Returns(CallWall.Google.Authorization.AuthorizationStatus.NotAuthorized);
            _testSchedulerProvider = new TestSchedulerProvider();
            _sut = new GoogleAccountSetupViewModel(_settingsMock.Object, _authorizationMock.Object, _testSchedulerProvider);
        }

        [Test]
        public void Changes_to_Authorization_Status_should_raise_propertyChanged_for_IsAuthorized()
        {
            var wasRaised = false;
            _sut.PropertyChanges(ga => ga.IsAuthorized).Subscribe(_ => { wasRaised = true; });

            _authorizationMock.Raise(a => a.PropertyChanged += null, new PropertyChangedEventArgs("Status"));
            _testSchedulerProvider.Dispatcher.Start();
            Assert.IsTrue(wasRaised);
        }
        [Test]
        public void Changes_to_Authorization_Status_should_raise_AuthorizeCommand_CanExecuteChanged()
        {
            var wasRaised = false;
            _sut.AuthorizeCommand.CanExecuteChanged += (s, e) => { wasRaised = true; };

            _authorizationMock.Raise(a => a.PropertyChanged += null, new PropertyChangedEventArgs("Status"));
            _testSchedulerProvider.Dispatcher.Start();
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void Changes_to_Authorization_Status_should_raise_propertyChanged_for_IsProcessing()
        {
            var wasRaised = false;
            _sut.PropertyChanges(ga => ga.IsProcessing).Subscribe(_ => { wasRaised = true; });

            _authorizationMock.Raise(a => a.PropertyChanged += null, new PropertyChangedEventArgs("Status"));
            _testSchedulerProvider.Dispatcher.Start();
            Assert.IsTrue(wasRaised);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Setting_IsEnabled_should_update_personalization_settings(bool isEnabled)
        {
            _sut.IsEnabled = isEnabled;
            _settingsMock.Verify(s => s.Put(_googleIsEnabledSettingsKey, isEnabled.ToString()));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Setting_IsEnabled_should_raise_propertychanged(bool isEnabled)
        {
            var wasRaised = false;
            _sut.PropertyChanges(ga => ga.IsEnabled).Subscribe(_ => { wasRaised = true; });

            _sut.IsEnabled = isEnabled;

            Assert.IsTrue(wasRaised);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Getting_IsEnabled_should_get_value_from_Settings(bool isEnabled)
        {
            _settingsMock.Setup(s => s.Get(_googleIsEnabledSettingsKey)).Returns(isEnabled.ToString());

            Assert.AreEqual(isEnabled, _sut.IsEnabled);
        }

        [Test]
        public void Resources_property_should_delegate_to_Authorization_AvailableResourceScopes()
        {
            var expected = new ReadOnlyCollection<GoogleResource>(new[] { GoogleResource.Contacts, GoogleResource.Gmail });
            _authorizationMock.SetupGet(s => s.AvailableResourceScopes).Returns(expected);

            CollectionAssert.AreEquivalent(expected, _sut.Resources);
        }

        [Test]
        public void Authorize_should_pass_SelectedResources_to_Authorization()
        {
            var expected = new[] { GoogleResource.Contacts, GoogleResource.Gmail };
            _authorizationMock.SetupGet(s => s.AvailableResourceScopes).Returns(GoogleResource.AvailableResourceScopes);
            _authorizationMock.Setup(a => a.Authorize(expected)).Returns(Observable.Empty<Unit>()).Verifiable("Didn't call Authorize with expected resources");

            foreach (var googleResource in expected)
            {
                _sut.SelectedResources.Add(googleResource);
            }

            _sut.AuthorizeCommand.Execute();

            _authorizationMock.Verify();
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_false_when_IsEnabled_is_false()
        {
            _settingsMock.Setup(s => s.Get(_googleIsEnabledSettingsKey)).Returns(false.ToString());

            Assert.IsFalse(_sut.AuthorizeCommand.CanExecute());
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_false_when_IsAuthorized_is_true()
        {
            _authorizationMock.SetupGet(m => m.Status).Returns(Google.Authorization.AuthorizationStatus.NotAuthorized);

            Assert.IsFalse(_sut.AuthorizeCommand.CanExecute());
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_true_when_IsEnabled_is_true_IsAuthorized_is_false()
        {
            _settingsMock.Setup(s => s.Get(_googleIsEnabledSettingsKey)).Returns(true.ToString());
            _authorizationMock.SetupGet(m => m.Status).Returns(Google.Authorization.AuthorizationStatus.NotAuthorized);

            Assert.IsTrue(_sut.AuthorizeCommand.CanExecute());
        }

    }
}
// ReSharper restore InconsistentNaming
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using CallWall.Google.AccountConfiguration;
using CallWall.Google.Authorization;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.AccountConfiguration
{
    [TestFixture]
    public sealed class GoogleAccountSetupFixture
    {
        private const string _googleIsEnabledSettingsKey = "CallWall.Google.AccountConfiguration.GoogleAccountSetup.IsEnabled";
        private GoogleAccountSetup _googleAccountSetup;
        private Mock<IPersonalizationSettings> _settingsMock;
        private Mock<IGoogleAuthorization> _authorizationMock;

        [SetUp]
        public void SetUp()
        {
            _settingsMock = new Mock<IPersonalizationSettings>();
            _authorizationMock = new Mock<IGoogleAuthorization>();
            _authorizationMock.SetupGet(a => a.Status).Returns(CallWall.Google.Authorization.AuthorizationStatus.NotAuthorized);
            _googleAccountSetup = new GoogleAccountSetup(_settingsMock.Object, _authorizationMock.Object);
        }

        [Test]
        public void Changes_to_Authorization_Status_should_raise_propertyChanged_for_IsAuthorized()
        {
            var wasRaised = false;
            _googleAccountSetup.PropertyChanges(ga => ga.IsAuthorized).Subscribe(_ => { wasRaised = true; });

            _authorizationMock.Raise(a => a.PropertyChanged += null, new PropertyChangedEventArgs("Status"));
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void Changes_to_Authorization_Status_should_raise_propertyChanged_for_IsProcessing()
        {
            var wasRaised = false;
            _googleAccountSetup.PropertyChanges(ga => ga.IsProcessing).Subscribe(_ => { wasRaised = true; });

            _authorizationMock.Raise(a => a.PropertyChanged += null, new PropertyChangedEventArgs("Status"));
            Assert.IsTrue(wasRaised);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Setting_IsEnabled_should_update_personalization_settings(bool isEnabled)
        {
            _googleAccountSetup.IsEnabled = isEnabled;
            _settingsMock.Verify(s => s.Put(_googleIsEnabledSettingsKey, isEnabled.ToString()));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Setting_IsEnabled_should_raise_propertychanged(bool isEnabled)
        {
            var wasRaised = false;
            _googleAccountSetup.PropertyChanges(ga => ga.IsEnabled).Subscribe(_ => { wasRaised = true; });

            _googleAccountSetup.IsEnabled = isEnabled;

            Assert.IsTrue(wasRaised);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Getting_IsEnabled_should_get_value_from_Settings(bool isEnabled)
        {
            _settingsMock.Setup(s => s.Get(_googleIsEnabledSettingsKey)).Returns(isEnabled.ToString());

            Assert.AreEqual(isEnabled, _googleAccountSetup.IsEnabled = isEnabled);
        }

        [Test]
        public void Resources_property_should_delegate_to_Authorization_AvailableResourceScopes()
        {
            var expected = new ReadOnlyCollection<GoogleResource>(new[] {GoogleResource.Contacts, GoogleResource.Gmail});
            _authorizationMock.SetupGet(s => s.AvailableResourceScopes).Returns(expected);
            
            CollectionAssert.AreEquivalent(expected, _googleAccountSetup.Resources);
        }

        [Test]
        public void Authorize_should_pass_SelectedResources_to_Authorization()
        {
            var expected = new[]{GoogleResource.Contacts, GoogleResource.Gmail};
            _authorizationMock.SetupGet(s => s.AvailableResourceScopes).Returns(GoogleResource.AvailableResourceScopes);
            _authorizationMock.Setup(a => a.Authorize(expected)).Returns(Observable.Empty<Unit>()).Verifiable("Didn't call Authorize with expected resources");
            
            foreach (var googleResource in expected)
            {
                _googleAccountSetup.SelectedResources.Add(googleResource);    
            }
            
            _googleAccountSetup.Authorize();

            _authorizationMock.Verify();
        }
    }
}
// ReSharper restore InconsistentNaming
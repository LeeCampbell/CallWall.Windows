using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CallWall.Google.AccountConfiguration;
using Moq;
using NUnit.Framework;

namespace CallWall.Google.UnitTests.AccountConfiguration
{
    [TestFixture]
    public sealed class GoogleAccountSetupViewModelFixture
    {
        private Mock<IGoogleAccountSetup> _modelMock;
        private GoogleAccountSetupViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _modelMock = new Mock<IGoogleAccountSetup>();
            _viewModel = new GoogleAccountSetupViewModel(_modelMock.Object);
        }

        [Test]
        public void When_model_IsAuthorized_changes_AuthorizeCommand_raises_CanExecuteChanged()
        {
            var wasRaised = false;
            _viewModel.AuthorizeCommand.CanExecuteChanged += (s, e) => { wasRaised = true; };

            _modelMock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsAuthorized"));

            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void When_model_IsEnabled_changes_AuthorizeCommand_raises_CanExecuteChanged()
        {
            var wasRaised = false;
            _viewModel.AuthorizeCommand.CanExecuteChanged += (s, e) => { wasRaised = true; };

            _modelMock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsEnabled"));

            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void When_model_IsEnabled_changes_raises_PropertyChanged_for_IsEnabled()
        {
            var wasRaised = false;
            _viewModel.PropertyChanges(vm => vm.IsEnabled).Subscribe(_ => { wasRaised = true; });

            _modelMock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsEnabled"));

            Assert.IsTrue(wasRaised);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsEnabled_Should_return_model_IsEnabled(bool isEnabled)
        {
            _modelMock.SetupGet(m => m.IsEnabled).Returns(isEnabled);

            Assert.AreEqual(isEnabled, _viewModel.IsEnabled);
        }

        [Test]
        public void AuthorizeCommand_Execute_should_call_model_Authorize()
        {
            _viewModel.AuthorizeCommand.Execute();

            _modelMock.Verify(m => m.Authorize());
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_false_when_IsEnabled_is_false()
        {
            _modelMock.SetupGet(m => m.IsEnabled).Returns(false);

            Assert.IsFalse(_viewModel.AuthorizeCommand.CanExecute());
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_false_when_IsAuthorized_is_true()
        {
            _modelMock.SetupGet(m => m.IsAuthorized).Returns(true);

            Assert.IsFalse(_viewModel.AuthorizeCommand.CanExecute());
        }

        [Test]
        public void AuthorizeCommand_CanExecute_should_return_true_when_IsEnabled_is_true_IsAuthorized_is_false()
        {
            _modelMock.SetupGet(m => m.IsEnabled).Returns(true);
            _modelMock.SetupGet(m => m.IsAuthorized).Returns(false);

            Assert.IsTrue(_viewModel.AuthorizeCommand.CanExecute());
        }

        [Test]
        public void Resources_should_return_model_Resources()
        {
            var expected = new ReadOnlyCollection<GoogleResource>(new[] {GoogleResource.Contacts, GoogleResource.Gmail});
            _modelMock.SetupGet(m => m.Resources).Returns(expected);

            CollectionAssert.AreEquivalent(expected, _viewModel.Resources);
        }

        [Test]
        public void SelectedResources_should_return_model_SelectedResources()
        {
            var expected = new ObservableCollection<GoogleResource>();
            _modelMock.SetupGet(m => m.SelectedResources).Returns(expected);

            CollectionAssert.AreEquivalent(expected, _viewModel.SelectedResources);
            
            _viewModel.SelectedResources.Add(GoogleResource.Gmail);
            
            CollectionAssert.AreEquivalent(expected, _viewModel.SelectedResources);
        }
    }
}
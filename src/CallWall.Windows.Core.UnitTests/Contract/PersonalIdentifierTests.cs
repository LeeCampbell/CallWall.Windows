using CallWall.Windows.Contract;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Core.UnitTests.Contract
{
    [TestFixture]
    public class PersonalIdentifierTests
    {
        private PersonalIdentifier _sut;
        private const string ExpectedIdentifierType = "email";
        private const string ExpectedValue = "Lee@home.com";
        private IProviderDescription _expectedProviderDescription;

        [SetUp]
        public void SetUp()
        {
            _expectedProviderDescription = new Mock<IProviderDescription>().Object;
            _sut = new PersonalIdentifier(ExpectedIdentifierType, ExpectedValue, _expectedProviderDescription);
        }

        [Test]
        public void Should_return_IdentifierType_from_ctor()
        {
            Assert.AreSame(ExpectedIdentifierType, _sut.IdentifierType);
        }

        [Test]
        public void Should_return_Value_from_ctor()
        {
            Assert.AreSame(ExpectedValue, _sut.Value);
        }

        [Test]
        public void Should_return_Provider_from_ctor()
        {
            Assert.AreSame(_expectedProviderDescription, _sut.Provider);
        }

    }
}
// ReSharper restore InconsistentNaming
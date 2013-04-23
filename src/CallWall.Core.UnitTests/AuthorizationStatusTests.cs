using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests
{
    [TestFixture]
    public sealed class AuthorizationStatusTests
    {
        [Test]
        public void Should_return_false_from_NotAuthorized_IsAuthorized()
        {
            Assert.IsFalse(AuthorizationStatus.NotAuthorized.IsAuthorized);
        }
        [Test]
        public void Should_return_false_from_NotAuthorized_IsProcessing()
        {
            Assert.IsFalse(AuthorizationStatus.NotAuthorized.IsProcessing);
        }

        [Test]
        public void Should_return_false_from_Authorized_IsProcessing()
        {
            Assert.IsFalse(AuthorizationStatus.Authorized.IsProcessing);
        }
        [Test]
        public void Should_return_true_from_Authorized_IsAuthorized()
        {
            Assert.IsTrue(AuthorizationStatus.Authorized.IsAuthorized);
        }

        [Test]
        public void Should_return_false_from_Processing_IsAuthorized()
        {
            Assert.IsFalse(AuthorizationStatus.Processing.IsAuthorized);
        }
        [Test]
        public void Should_return_true_from_Processing_IsProcessing()
        {
            Assert.IsTrue(AuthorizationStatus.Processing.IsProcessing);
        }
    }
}
// ReSharper restore InconsistentNaming
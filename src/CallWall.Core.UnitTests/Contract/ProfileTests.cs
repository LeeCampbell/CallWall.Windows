using CallWall.Contract;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests.Contract
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void Should_return_values_to_ctor_from_Identifiers_property()
        {
            var value1 = CreatePersonalIdentifier();
            var value2 = CreatePersonalIdentifier();
            IEnumerable<IPersonalIdentifier> values = new[] { value1, value2 };
            var sut = new Profile(values);

            CollectionAssert.AreEqual(values, sut.Identifiers);
        }

        private static IPersonalIdentifier CreatePersonalIdentifier()
        {
            return new Mock<IPersonalIdentifier>().Object;
        }
    }
}
// ReSharper restore InconsistentNaming
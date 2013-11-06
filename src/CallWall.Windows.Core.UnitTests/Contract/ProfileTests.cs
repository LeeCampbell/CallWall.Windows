using CallWall.Windows.Contract;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Core.UnitTests.Contract
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

        [Test]
        public void Should_return_copy_of_values_to_ctor_from_Identifiers_property()
        {
            var value1 = CreatePersonalIdentifier();
            var value2 = CreatePersonalIdentifier();
            var values = new List<IPersonalIdentifier> { value1, value2 };
            var sut = new Profile(values);
            var expected = values.ToArray();
            values.Add(CreatePersonalIdentifier());

            CollectionAssert.AreEqual(expected, sut.Identifiers);
        }

        private static IPersonalIdentifier CreatePersonalIdentifier()
        {
            return new Mock<IPersonalIdentifier>().Object;
        }
    }
}
// ReSharper restore InconsistentNaming
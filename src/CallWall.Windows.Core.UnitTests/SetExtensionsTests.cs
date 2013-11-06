using System.Collections.Generic;
using NUnit.Framework;

namespace CallWall.Windows.Core.UnitTests
{
    [TestFixture]
    public sealed class SetExtensionsTests
    {
        [Test]
        public void AddRange_should_add_provided_values_to_target_Set()
        {
            var target = new HashSet<int>();    
            var source = new[] {1, 2, 3, 4, 5};
            target.AddRange(source);

            CollectionAssert.AreEquivalent(source, target);
        }
    }
}

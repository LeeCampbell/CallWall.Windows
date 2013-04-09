using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace CallWall.Core.UnitTests
{
    public abstract class Given_a_BooleanToInvisibilityConverter
    {
        private BooleanToInvisibilityConverter _sut;

        private Given_a_BooleanToInvisibilityConverter()
        {}

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new BooleanToInvisibilityConverter();
        }

        [TestFixture]
        public sealed class When_converting_a_boolean_value : Given_a_BooleanToInvisibilityConverter
        {
            [Test]
            public void Should_return_Visible_for_false_values()
            {
                var actual = _sut.Convert(false, typeof (Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(Visibility.Visible, actual);
            }

            [Test]
            public void Should_return_Collapsed_for_true_values()
            {
                var actual = _sut.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(Visibility.Collapsed, actual);
            } 
        }

        [TestFixture]
        public sealed class When_converting_a_nullable_boolean_value : Given_a_BooleanToInvisibilityConverter
        {
            [Test]
            public void Should_return_Visible_for_false_values()
            {
                bool? value = false;
                var actual = _sut.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(Visibility.Visible, actual);
            }

            [Test]
            public void Should_return_Collapsed_for_true_values()
            {
                bool? value = true;
                var actual = _sut.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(Visibility.Collapsed, actual);
            }

            [Test]
            public void Should_return_Visible_for_true_values()
            {
                bool? value = null;
                var actual = _sut.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(Visibility.Visible, actual);
            }
        }

        [TestFixture]
        public sealed class When_converting_back : Given_a_BooleanToInvisibilityConverter
        {
            [Test]
            public void Should_return_false_for_Visible_values()
            {
                var actual = _sut.ConvertBack(Visibility.Visible, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(false, actual);
            }

            [Test]
            public void Should_return_true_for_Collapsed_values()
            {
                var actual = _sut.ConvertBack(Visibility.Collapsed, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(true, actual);
            }

            [TestCase(Visibility.Hidden)]
            [TestCase(-1)]
            [TestCase("Some string")]
            public void Should_return_false_for_unsupported_values(object value)
            {
                var actual = _sut.ConvertBack(value, typeof(Visibility), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(false, actual);
            }
        }
    }
}

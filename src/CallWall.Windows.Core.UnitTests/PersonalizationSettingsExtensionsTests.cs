using System;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Windows.Core.UnitTests
{
    public abstract class Given_an_IPersonalizationSettings_instance
    {
        private Given_an_IPersonalizationSettings_instance()
        { }

        private Mock<IPersonalizationSettings> _personalizationSettingsMock;

        [SetUp]
        public virtual void SetUp()
        {
            _personalizationSettingsMock = new Mock<IPersonalizationSettings>();
        }

        [TestFixture]
        public sealed class When_getting_a_boolean_value : Given_an_IPersonalizationSettings_instance
        {
            private const string Key = "BoolKey";

            [Test]
            public void Should_return_true_string_as_true()
            {
                _personalizationSettingsMock.Setup(ps => ps.Get(Key)).Returns("true");

                Assert.IsTrue(_personalizationSettingsMock.Object.GetAsBool(Key, false));
            }

            [Test]
            public void Should_return_false_string_as_false()
            {
                _personalizationSettingsMock.Setup(ps => ps.Get(Key)).Returns("false");

                Assert.IsFalse(_personalizationSettingsMock.Object.GetAsBool(Key, true));
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_return_null_value_as_provided_fallbackvalue(bool fallback)
            {
                _personalizationSettingsMock.Setup(ps => ps.Get(Key)).Returns((string)null);

                Assert.AreEqual(fallback, _personalizationSettingsMock.Object.GetAsBool(Key, fallback));
            }

            [TestCase(false)]
            [TestCase(true)]
            public void Should_throw_if_value_is_not_a_boolean(bool fallback)
            {
                _personalizationSettingsMock.Setup(ps => ps.Get(Key)).Returns("Not a bool");
                Assert.Throws<FormatException>(() => _personalizationSettingsMock.Object.GetAsBool(Key, fallback));
            }
        }

        [TestFixture]
        public sealed class When_setting_a_boolean_value : Given_an_IPersonalizationSettings_instance
        {
            private const string Key = "BoolKey";

            [Test]
            public void Should_pass_true_as_true_string()
            {
                _personalizationSettingsMock.Setup(ps => ps.Put(Key, "true"));
                _personalizationSettingsMock.Object.SetAsBool(Key, true);

                _personalizationSettingsMock.Verify();
            }

            [Test]
            public void Should_pass_false_as_false_string()
            {
                _personalizationSettingsMock.Setup(ps => ps.Put(Key, "false"));

                _personalizationSettingsMock.Object.SetAsBool(Key, false);

                _personalizationSettingsMock.Verify();
            }
        }
    }
}
// ReSharper restore InconsistentNaming
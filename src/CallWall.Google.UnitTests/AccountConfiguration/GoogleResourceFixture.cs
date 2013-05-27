using System;
using CallWall.Google.AccountConfiguration;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.AccountConfiguration
{
    [TestFixture]
    public sealed class GoogleResourceFixture
    {
        [Test]
        public void Should_return_GoogleContact_values()
        {
            Assert.AreEqual("Contacts", GoogleResource.Contacts.Name);
            Assert.AreEqual(new Uri("pack://application:,,,/CallWall.Google;component/Images/Contacts_48x48.png"), GoogleResource.Contacts.Image);
            Assert.AreEqual(new Uri(@"https://www.google.com/m8/feeds/"), GoogleResource.Contacts.Resource);
        }

        [Test]
        public void Should_return_Gmail_values()
        {
            Assert.AreEqual("Email", GoogleResource.Gmail.Name);
            Assert.AreEqual(new Uri("pack://application:,,,/CallWall.Google;component/Images/Email_48x48.png"), GoogleResource.Gmail.Image);
            Assert.AreEqual(new Uri(@"https://mail.google.com/"), GoogleResource.Gmail.Resource);
        }

        [Test]
        public void Should_return_Contacts_and_Gmail_from_AvailableResourceScopes()
        {
            Assert.AreEqual(GoogleResource.Contacts, GoogleResource.AvailableResourceScopes[0]);
            Assert.AreEqual(GoogleResource.Gmail, GoogleResource.AvailableResourceScopes[1]);
        }
    }
}
// ReSharper restore InconsistentNaming
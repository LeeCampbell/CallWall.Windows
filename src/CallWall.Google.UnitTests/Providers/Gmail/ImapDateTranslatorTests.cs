using CallWall.Google.Providers.Gmail.Imap;
using NUnit.Framework;

namespace CallWall.Google.UnitTests.Providers.Gmail
{
    [TestFixture]
    public sealed class ImapDateTranslatorTests
    {
        [TestCase("1 Jan 2000 12:00:00 +12:00", "2000-01-01T12:00:00.0000000+12:00")]
        [TestCase("Mon, 1 Jan 2000 12:00:00 +12:00", "2000-01-01T12:00:00.0000000+12:00")]
        [TestCase("1 Jan 2000 12:00:00 +12:00 DST", "2000-01-01T12:00:00.0000000+12:00")]
        [TestCase("Mon, 1 Jan 2000 12:00:00 +12:00 DST", "2000-01-01T12:00:00.0000000+12:00")]  
        
        [TestCase("Thu, 23 May 2013 10:40:11 +12:00 (NZST)", "2013-05-23T10:40:11.0000000+12:00")]
        [TestCase("Sun, 9 Jun 2013 21:00:47 +0000 (GMT)", "2013-06-09T21:00:47.0000000+00:00")]

        [TestCase("1 JAN 2000 12:00:00 +12:00", "2000-01-01T12:00:00.0000000+12:00")]
        [TestCase("1 jan 2000 12:00:00 +12:00", "2000-01-01T12:00:00.0000000+12:00")]
        [TestCase("1 Feb 2000 12:00:00 +12:00", "2000-02-01T12:00:00.0000000+12:00")]
        [TestCase("1 Mar 2000 12:00:00 +12:00", "2000-03-01T12:00:00.0000000+12:00")]
        [TestCase("1 Apr 2000 12:00:00 +12:00", "2000-04-01T12:00:00.0000000+12:00")]
        [TestCase("1 May 2000 12:00:00 +12:00", "2000-05-01T12:00:00.0000000+12:00")]
        [TestCase("1 Jun 2000 12:00:00 +12:00", "2000-06-01T12:00:00.0000000+12:00")]
        [TestCase("1 Jul 2000 12:00:00 +12:00", "2000-07-01T12:00:00.0000000+12:00")]
        [TestCase("1 Aug 2000 12:00:00 +12:00", "2000-08-01T12:00:00.0000000+12:00")]
        [TestCase("1 Sep 2000 12:00:00 +12:00", "2000-09-01T12:00:00.0000000+12:00")]
        [TestCase("1 Oct 2000 12:00:00 +12:00", "2000-10-01T12:00:00.0000000+12:00")]
        [TestCase("1 Nov 2000 12:00:00 +12:00", "2000-11-01T12:00:00.0000000+12:00")]
        [TestCase("1 Dec 2000 12:00:00 +12:00", "2000-12-01T12:00:00.0000000+12:00")]  
        public void Should_translate_exptected_date_formats(string input, string expected)
        {
            var sut = new ImapDateTranslator();
            var actual = sut.Translate(input);
            var isoStringActual = actual.ToString("o");
            Assert.AreEqual(expected, isoStringActual);
        }
    }
}

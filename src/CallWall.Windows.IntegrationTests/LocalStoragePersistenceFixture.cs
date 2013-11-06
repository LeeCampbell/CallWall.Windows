using NUnit.Framework;

namespace CallWall.Windows.IntegrationTests
{
    [TestFixture]
    public sealed class LocalStoragePersistenceFixture
    {
        private LocalStoragePersistence _sut = new LocalStoragePersistence();
        private const string FileName = "SomeFile";


        [SetUp]
        public void SetUp()
        {
            _sut = new LocalStoragePersistence();
            _sut.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            _sut.Reset();
        }

        [Test]
        public void Should_return_empty_string_for_unknown_file()
        {
            var actual = _sut.Read(FileName);
            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void Should_be_able_read_its_own_write()
        {
            var expected = "Testing the reader-writer";

            _sut.Write(FileName, expected);
            var actual = _sut.Read(FileName);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void After_Write_Reset_should_return_empty_from_read()
        {
            var expected = "Testing the reader-writer";

            _sut.Write(FileName, expected);
            _sut.Reset();

            var actual = _sut.Read(FileName);
            Assert.IsNullOrEmpty(actual);
        }
    }
}

using System;
using NUnit.Framework;

namespace CallWall.UnitTests
{
    [TestFixture]
    public class JsonSerializerFixture
    {
        [Test]
        public void Can_serialize_to_Json_format()
        {
            var expected = "{\r\n  \"DateOfBirth\": \"1980-06-15T00:00:00\",\r\n  \"Name\": \"Fred\",\r\n  \"Rank\": 3\r\n}";
            var expectedRank = 3;
            var expectedName = "Fred";
            var expectedDOB = new DateTime(1980, 6, 15);
            var data = new Person
            {
                Rank = expectedRank,
                Name = expectedName,
                DateOfBirth = expectedDOB
            };
            
            var sut = new JsonSerializer();
            var payload = sut.Serialize(data);
            Assert.AreEqual(expected, payload);
        }

        [Test]
        public void Can_Deserialize_from_Json_format()
        {
            var source = "{\r\n  \"DateOfBirth\": \"1980-06-15T00:00:00\",\r\n  \"Name\": \"Fred\",\r\n  \"Rank\": 3\r\n}";
            var sut = new JsonSerializer();
            var actual = sut.Deserialize<Person>(source);
            Assert.AreEqual(3, actual.Rank);
            Assert.AreEqual("Fred", actual.Name);
            Assert.AreEqual(new DateTime(1980, 6, 15), actual.DateOfBirth);
        }

        [Test]
        public void Can_serialize_and_deserialize_back()
        {
            var expectedRank = 3;
            var expectedName = "Fred";
            var expectedDOB = new DateTime(1980, 6, 15);
            var data = new Person
                {
                    Rank = expectedRank,
                    Name = expectedName,
                    DateOfBirth = expectedDOB
                };
            var sut = new JsonSerializer();
            var payload = sut.Serialize(data);
            var actual = sut.Deserialize<Person>(payload);

            Assert.AreEqual(expectedRank, actual.Rank);
            Assert.AreEqual(expectedName, actual.Name);
            Assert.AreEqual(expectedDOB, actual.DateOfBirth);
        }
    }

    public class Person
    {
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}
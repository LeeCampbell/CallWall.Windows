using System;
using System.Globalization;
using CallWall.Controls;
using NUnit.Framework;

namespace CallWall.UnitTests.Controls
{
    //TODO: This implementation still needs to be done, along with the birthday control. 
    public abstract class Given_a_BirthdayTextConverter
    {
        private BirthdayTextConverter _sut;

        public virtual void Setup()
        {
            _sut = new BirthdayTextConverter();
        }

        //TODO: I want to be able to say
        // 40th was last [weekday]
        // 40th was [week]
        // 40th was yesterday
        // 40th today
        // 40th tomorrow
        // 40th this [weekday]
        // 40th next [weekday]
        // 40th in 10 days
        // 40th next month
        // (40)


        //This may almost be easier splitting it into more part.
        //  AgeConverter e.g. 40-->40th, 1-->1st, 23-->23rd
        //  Proximity converter, was it LastWeek/wasThisWeek/yesterday/Today/Tomorrow/IsThisWeek/IsNextWeek

        [TestFixture]
        public sealed class When_Birthday_was_yesterday : Given_a_BirthdayTextConverter
        {
            [Test, Ignore]
            public void Should_return_CurrentAge_was_yesterday()
            {
                Assert.Fail();
            }
        }

        [TestFixture, Ignore]
        public sealed class When_Birthday_was_Last_week : Given_a_BirthdayTextConverter
        {
            [TestCase(40, -2, DayOfWeek.Monday, "40th last Saturday")]
            [TestCase(40, -2, DayOfWeek.Tuesday, "40th last Saturday")]
            [TestCase(40, -2, DayOfWeek.Wednesday, "40th last Saturday")]
            [TestCase(40, -2, DayOfWeek.Thursday, "40th last Saturday")]
            [TestCase(40, -7, DayOfWeek.Friday, "40th last Friday")]
            [TestCase(40, -7, DayOfWeek.Saturday, "40th last Friday")]
            [TestCase(40, -7, DayOfWeek.Sunday, "40th last Friday")]
            public void Should_return_Current_age_last_weekday(int age, int daysOffset, DayOfWeek currentDay, string expected)
            {
                //Sunday is DayOfWeek=0, so we use 1st Jan 2006 as our root as it was a Sunday. Makes the math easier.
                var today = new DateTime(2006, 1, 1).AddDays((int)currentDay);
                var birthdate = today.AddYears(age - 1).AddDays(daysOffset);
                _sut = new BirthdayTextConverter(() => today);

                var actual = _sut.Convert(birthdate, typeof(string), null, CultureInfo.InvariantCulture);
                Assert.AreEqual(expected, actual);
            }

        }
    }
}

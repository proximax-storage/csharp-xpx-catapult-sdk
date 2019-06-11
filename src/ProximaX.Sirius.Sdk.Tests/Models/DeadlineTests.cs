using System;
using FluentAssertions;
using ProximaX.Sirius.Sdk.Model.Transactions;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Models
{
    public class DeadlineTests
    {
        [Fact]
        public void Should_Throw_Exception_If_Deadline_Smaller_Than_Timestamp()
        {
            Action act = () => Deadline.Create(-2);

            act.Should().Throw<Exception>().WithMessage("Deadline should be greater than 0");

        }
        /*
        [Fact]
        public void Should_Create_Deadline_For_Two_Hours_From_Now()
        {
            var now = DateTime.Now.ToLocalTime();
            var deadline = Deadline.Create();

            var localDate = deadline.GetLocalDateTime();
            var nowPlus = now.AddHours(1).AddSeconds(2);
            now.CompareTo(localDate).Should().BeLessThan(0, @"now is before deadline local time)");
            now.AddHours(1).AddMinutes(-1).CompareTo(localDate).Should()
                .BeLessThan(0, "now plus 2 hours is before deadline localtime");
            nowPlus.CompareTo(localDate).Should()
                .BeGreaterThan(0, "now plus 2 hours and 2 seconds is after deadline localtime");
        }

        
        [Fact]
        public void Should_Create_Deadline_For_70_Minutes_From_Now()
        {
            var now = DateTime.Now.ToLocalTime();
            var deadline = Deadline.Create(70, ChronoUnit.MINUTES);
            const int minutes = 70;
            var localDate = deadline.GetLocalDateTime();
            var nowPlus = now.AddMinutes(minutes).AddSeconds(2);

            now.CompareTo(localDate).Should().BeLessThan(0, @"now is before deadline local time)");
            now.AddHours(1).AddMinutes(minutes).CompareTo(localDate).Should()
                .BeLessThan(0, "now plus 2 hours is before deadline localtime");
            nowPlus.CompareTo(localDate).Should()
                .BeGreaterThan(0, "now plus 2 hours and 2 seconds is after deadline localtime");
        }

        [Fact]
        public void Should_Create_Deadline_For_Two_Hours_From_Now_With_TimeZone()
        {
            var timeNow = DateTime.Now;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTimeNow = TimeZoneInfo.ConvertTime(timeNow, TimeZoneInfo.Local,
                                                   easternZone);

            var deadline = Deadline.Create();

            var localDate = deadline.GetLocalDateTime(easternZone);
            var nowPlus = easternTimeNow.AddHours(1).AddSeconds(2);
            easternTimeNow.CompareTo(localDate).Should().BeLessThan(0, @"now is before deadline local time)");
            easternTimeNow.AddHours(1).AddMinutes(-1).CompareTo(localDate).Should()
                .BeLessThan(0, "now plus 2 hours is before deadline localtime");
            nowPlus.CompareTo(localDate).Should()
                .BeGreaterThan(0, "now plus 2 hours and 2 seconds is after deadline localtime");

        }*/
    }
}

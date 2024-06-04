// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class DateTimeRangeTests
    {
        private readonly ITestOutputHelper output;

        public DateTimeRangeTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DateTimeRange_Constructor_StartIsBeforeEnd_CreatesInstance()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));

            Assert.Equal(new DateTime(2023, 1, 1), range.Start);
            Assert.Equal(new DateTime(2023, 1, 3), range.End);
        }

        [Fact]
        public void DateTimeRange_Constructor_StartIsAfterEnd_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new DateTimeRange(new DateTime(2023, 1, 3), new DateTime(2023, 1, 1)));
        }

        [Fact]
        public void DateTimeRange_IsTimePoint_RangeIsSinglePointInTime_ReturnsTrue()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));

            Assert.True(range.IsTimePoint);
        }

        [Fact]
        public void DateTimeRange_IsTimePoint_RangeIsNotSinglePointInTime_ReturnsFalse()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 2));

            Assert.False(range.IsTimePoint);
        }

        [Fact]
        public void DateTimeRange_Duration_RangeIsSinglePointInTime_ReturnsZero()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));

            Assert.Equal(TimeSpan.Zero, range.Duration);
        }

        [Fact]
        public void DateTimeRange_Duration_RangeIsNotSinglePointInTime_ReturnsDuration()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));

            Assert.Equal(TimeSpan.FromDays(2), range.Duration);
        }

        [Fact]
        public void DateTimeRange_Overlaps_RangesDoNotOverlap_ReturnsFalse()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 4), new DateTime(2023, 1, 6));
            TimeSpan minimumOverlap = TimeSpan.FromDays(1);

            Assert.False(DateTimeRange.Overlaps(range1, range2, minimumOverlap));
            Assert.False(range1.Overlaps(range2, minimumOverlap));
        }

        [Fact]
        public void DateTimeRange_Overlaps_RangesOverlapButBelowMinimumOverlap_ReturnsFalse()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 10));
            TimeSpan minimumOverlap = TimeSpan.FromDays(2);

            Assert.False(DateTimeRange.Overlaps(range1, range2, minimumOverlap));
            Assert.False(range1.Overlaps(range2, minimumOverlap));
        }

        [Fact]
        public void DateTimeRange_Overlaps_RangesOverlapExactlyAtMinimumOverlap_ReturnsTrue()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 4));
            TimeSpan minimumOverlap = TimeSpan.FromDays(1);

            Assert.True(DateTimeRange.Overlaps(range1, range2, minimumOverlap));
            Assert.True(range1.Overlaps(range2, minimumOverlap));
        }

        [Fact]
        public void DateTimeRange_Overlaps_RangesOverlapAboveMinimumOverlap_ReturnsTrue()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 4));
            TimeSpan minimumOverlap = TimeSpan.FromDays(1);

            Assert.True(DateTimeRange.Overlaps(range1, range2, minimumOverlap));
            Assert.True(range1.Overlaps(range2, minimumOverlap));
        }

        [Fact]
        public void DateTimeRange_Contains_DateTimeRangeContainsDateTime_ReturnsTrue()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTime dateTime = new DateTime(2023, 1, 2);

            Assert.True(DateTimeRange.Contains(range, dateTime));
            Assert.True(range.Contains(dateTime));
        }

        [Fact]
        public void DateTimeRange_Contains_DateTimeRangeDoesNotContainDateTime_ReturnsFalse()
        {
            DateTimeRange range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTime dateTime = new DateTime(2023, 1, 4);

            Assert.False(DateTimeRange.Contains(range, dateTime));
            Assert.False(range.Contains(dateTime));
        }

        [Fact]
        public void DateTimeRange_Contains_DateTimeRangeContainsDateTimeRange_ReturnsTrue()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 3));

            Assert.True(DateTimeRange.Contains(range1, range2));
            Assert.True(range1.Contains(range2));
        }

        [Fact]
        public void DateTimeRange_Contains_DateTimeRangeDoesNotContainDateTimeRange_ReturnsFalse()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 4));

            Assert.False(DateTimeRange.Contains(range1, range2));
            Assert.False(range1.Contains(range2));
        }

        [Fact]
        public void DateTimeRange_Contains_DateTimeRangeIsSame_ReturnsTrue()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));

            Assert.True(DateTimeRange.Contains(range1, range2));
            Assert.True(range1.Contains(range2));
        }

        [Fact]
        public void DateTimeRange_Intersection_RangesDoNotOverlap_ReturnsNull()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 4), new DateTime(2023, 1, 6));

            Assert.Null(DateTimeRange.Intersection(range1, range2));
            Assert.Null(range1.Intersection(range2));
        }

        [Fact]
        public void DateTimeRange_Intersection_RangesOverlap_ReturnsCorrectly()
        {
            DateTimeRange range1 = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));
            DateTimeRange range2 = new DateTimeRange(new DateTime(2023, 1, 2), new DateTime(2023, 1, 4));

            DateTimeRange? intersection = DateTimeRange.Intersection(range1, range2);
            Assert.NotNull(intersection);

            Assert.Equal(new DateTime(2023, 1, 2), intersection?.Start);
            Assert.Equal(new DateTime(2023, 1, 3), intersection?.End);
        }

        [Fact]
        public void DateTimeRange_JSON_SerializeAndDeserialize_Correctly()
        {
            var range = new DateTimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 3));

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(range, options);
            output.WriteLine(json);

            var deserializedRange = JsonSerializer.Deserialize<DateTimeRange>(json, options);
            Assert.Equal(range, deserializedRange);
        }
    }
}
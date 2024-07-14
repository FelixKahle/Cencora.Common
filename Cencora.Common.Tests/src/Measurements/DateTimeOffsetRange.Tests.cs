// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Measurements;

namespace Cencora.Common.Tests.Measurements;

public class DateTimeOffsetRangeTests
{
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        var dateTimeRange = new DateTimeOffsetRange();

        Assert.Equal(DateTimeOffset.MinValue, dateTimeRange.Start);
        Assert.Equal(DateTimeOffset.MaxValue, dateTimeRange.End);
    }

    [Fact]
    public void Constructor_WithEndAfterStart_ShouldCreateInstance()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange = new DateTimeOffsetRange
        {
            Start = start,
            End = end
        };

        Assert.Equal(start, dateTimeRange.Start);
        Assert.Equal(end, dateTimeRange.End);
    }

    [Fact]
    public void Constructor_WithEndEqualStart_ShouldCreateInstance()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 1);

        var dateTimeRange = new DateTimeOffsetRange(start, end);

        Assert.Equal(start, dateTimeRange.Start);
        Assert.Equal(end, dateTimeRange.End);
    }

    [Fact]
    public void IsTimePoint_WithStartEqualEnd_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 1);

        var dateTimeRange = new DateTimeOffsetRange(start, end);

        Assert.True(dateTimeRange.IsTimePoint);
    }

    [Fact]
    public void ToUniversalTime_WithLocalTime_ShouldReturnUniversalTime()
    {
        var start = DateTime.Today.AddHours(6);
        var end = DateTime.Today.AddHours(12);

        var dateTimeRange = new DateTimeOffsetRange(start, end);

        Assert.Equal(start.ToUniversalTime(), dateTimeRange.Start.ToUniversalTime());
        Assert.Equal(end.ToUniversalTime(), dateTimeRange.End.ToUniversalTime());
    }

    [Fact]
    public void Duration_WithDifferentStartAndEnd_ShouldReturnDuration()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange = new DateTimeOffsetRange(start, end);

        Assert.Equal(TimeSpan.FromDays(1), dateTimeRange.Duration);
    }

    [Fact]
    public void Duration_WithEqualStartAndEnd_ShouldReturnZero()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 1);

        var dateTimeRange = new DateTimeOffsetRange(start, end);

        Assert.Equal(TimeSpan.Zero, dateTimeRange.Duration);
    }

    [Fact]
    public void Equals_WithEqualInstances_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.True(dateTimeRange1.Equals(dateTimeRange2));
        Assert.True(dateTimeRange1 == dateTimeRange2);
        Assert.False(dateTimeRange1 != dateTimeRange2);
    }

    [Fact]
    public void Equals_WithDifferentInstances_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.False(dateTimeRange1.Equals(dateTimeRange2));
        Assert.False(dateTimeRange1 == dateTimeRange2);
        Assert.True(dateTimeRange1 != dateTimeRange2);
    }

    [Fact]
    public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.Equal(dateTimeRange1.GetHashCode(), dateTimeRange2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.NotEqual(dateTimeRange1.GetHashCode(), dateTimeRange2.GetHashCode());
    }

    [Fact]
    public void Less_WithSmallerDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.True(dateTimeRange1 < dateTimeRange2);
    }

    [Fact]
    public void Less_WithBiggerDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.False(dateTimeRange2 < dateTimeRange1);
    }

    [Fact]
    public void Less_WithEqualDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.False(dateTimeRange1 < dateTimeRange2);
    }

    [Fact]
    public void LessOrEqual_WithSmallerDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.True(dateTimeRange1 <= dateTimeRange2);
    }

    [Fact]
    public void LessOrEqual_WithBiggerDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.False(dateTimeRange2 <= dateTimeRange1);
    }

    [Fact]
    public void LessOrEqual_WithEqualDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.True(dateTimeRange1 <= dateTimeRange2);
    }

    [Fact]
    public void Greater_WithBiggerDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.True(dateTimeRange2 > dateTimeRange1);
    }

    [Fact]
    public void Greater_WithSmallerDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.False(dateTimeRange1 > dateTimeRange2);
    }

    [Fact]
    public void Greater_WithEqualDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.False(dateTimeRange1 > dateTimeRange2);
    }

    [Fact]
    public void GreaterOrEqual_WithBiggerDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.True(dateTimeRange2 >= dateTimeRange1);
    }

    [Fact]
    public void GreaterOrEqual_WithSmallerDuration_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end.AddDays(1));

        Assert.False(dateTimeRange1 >= dateTimeRange2);
    }

    [Fact]
    public void GreaterOrEqual_WithEqualDuration_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 2);

        var dateTimeRange1 = new DateTimeOffsetRange(start, end);
        var dateTimeRange2 = new DateTimeOffsetRange(start, end);

        Assert.True(dateTimeRange1 >= dateTimeRange2);
    }

    [Fact]
    public void Overlaps_WithOverlappingRangesAndFittingTimespan_ShouldReturnTrue()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 3);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.True(DateTimeOffsetRange.Overlaps(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(1)));
        Assert.True(dateTimeRange1.Overlaps(dateTimeRange2, TimeSpan.FromDays(1)));
    }

    [Fact]
    public void Overlaps_WithOverlappingRangesAndNonFittingTimespan_ShouldReturnFalse()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 3);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.False(DateTimeOffsetRange.Overlaps(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(2)));
        Assert.False(dateTimeRange1.Overlaps(dateTimeRange2, TimeSpan.FromDays(2)));
    }

    [Fact]
    public void Overlaps_WithoutOverlappingRangesAndNonFittingTimespan_ShouldReturnFalse()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 2);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 3);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.False(DateTimeOffsetRange.Overlaps(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(1)));
        Assert.False(dateTimeRange1.Overlaps(dateTimeRange2, TimeSpan.FromDays(2)));
    }

    [Fact]
    public void Overlaps_WithoutOverlappingRanges_ShouldReturnFalse()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 2);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 3);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.False(DateTimeOffsetRange.Overlaps(dateTimeRange1, dateTimeRange2));
        Assert.False(dateTimeRange1.Overlaps(dateTimeRange2));
    }

    [Fact]
    public void Overlaps_WithOverlappingRanges_ShouldReturnTrue()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 2);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 1);
        var end2 = new DateTime(2024, 1, 3);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.True(DateTimeOffsetRange.Overlaps(dateTimeRange1, dateTimeRange2));
        Assert.True(dateTimeRange1.Overlaps(dateTimeRange2));
    }

    [Fact]
    public void Contains_WithTimePointInRange_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 3);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var point = new DateTime(2024, 1, 2); 

        Assert.True(DateTimeOffsetRange.Contains(dateTimeRange, point));
        Assert.True(dateTimeRange.Contains(point));
    }

    [Fact]
    public void Contains_WithTimePointNotInRange_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 3);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var point = new DateTime(2024, 1, 4); 

        Assert.False(DateTimeOffsetRange.Contains(dateTimeRange, point));
        Assert.False(dateTimeRange.Contains(point));
    }

    [Fact]
    public void Contains_WithTimePointInCompleteRange_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 1);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var point = new DateTime(2024, 1, 1); 

        Assert.True(DateTimeOffsetRange.Contains(dateTimeRange, point));
        Assert.True(dateTimeRange.Contains(point));
    }

    [Fact]
    public void Contains_WithTimeRangeInRange_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 5);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 3);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.True(DateTimeOffsetRange.Contains(dateTimeRange, dateTimeRange2));
        Assert.True(dateTimeRange.Contains(dateTimeRange2));
    }

    [Fact]
    public void Contains_WithTimeRangeNotInRange_ShouldReturnFalse()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 5);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 6);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.False(DateTimeOffsetRange.Contains(dateTimeRange, dateTimeRange2));
        Assert.False(dateTimeRange.Contains(dateTimeRange2));
    }

    [Fact]
    public void Contains_WithTimeRangeInCompleteRange_ShouldReturnTrue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 1);
        var dateTimeRange = new DateTimeOffsetRange(start, end);

        var start2 = new DateTime(2024, 1, 1);
        var end2 = new DateTime(2024, 1, 1);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        Assert.True(DateTimeOffsetRange.Contains(dateTimeRange, dateTimeRange2));
        Assert.True(dateTimeRange.Contains(dateTimeRange2));
    }

    [Fact]
    public void Intersection_WithOverlappingRangesAndFittingTimespan_ShouldReturnIntersection()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 3);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        var expectedStart = new DateTime(2024, 1, 2);
        var expectedEnd = new DateTime(2024, 1, 3);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(1));
        Assert.NotNull(intersection);
        Assert.Equal(expectedStart, intersection.Value.Start);
        Assert.Equal(expectedEnd, intersection.Value.End);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2, TimeSpan.FromDays(1));
        Assert.NotNull(intersection);
        Assert.Equal(expectedStart, intersection.Value.Start);
        Assert.Equal(expectedEnd, intersection.Value.End);
    }

    [Fact]
    public void Intersection_WithOverlappingRangesAndNonFittingTimespan_ShouldReturnNull()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 3);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 2);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(2));
        Assert.Null(intersection);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2, TimeSpan.FromDays(2));
        Assert.Null(intersection);
    }

    [Fact]
    public void Intersection_WithoutOverlappingRangesAndNonFittingTimespan_ShouldReturnNull()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 2);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 3);
        var end2 = new DateTime(2024, 1, 4);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2, TimeSpan.FromDays(2));
        Assert.Null(intersection);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2, TimeSpan.FromDays(2));
        Assert.Null(intersection);
    }

    [Fact]
    public void Intersection_WithOverlappingRanges_ShouldReturnIntersection()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 5);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 4);
        var end2 = new DateTime(2024, 1, 6);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        var expectedStart = new DateTime(2024, 1, 4);
        var expectedEnd = new DateTime(2024, 1, 5);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2);
        Assert.NotNull(intersection);
        Assert.Equal(expectedStart, intersection.Value.Start);
        Assert.Equal(expectedEnd, intersection.Value.End);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2);
        Assert.NotNull(intersection);
        Assert.Equal(expectedStart, intersection.Value.Start);
        Assert.Equal(expectedEnd, intersection.Value.End);
    }

    [Fact]
    public void Intersection_WithNonOverlappingRanges_ShouldReturnNull()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 5);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 6);
        var end2 = new DateTime(2024, 1, 7);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2);
        Assert.Null(intersection);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2);
        Assert.Null(intersection);
    }

    [Fact]
    public void Intersection_WithEqualRanges_ShouldReturnIntersection()
    {
        var start1 = new DateTime(2024, 1, 1);
        var end1 = new DateTime(2024, 1, 5);
        var dateTimeRange1 = new DateTimeOffsetRange(start1, end1);

        var start2 = new DateTime(2024, 1, 1);
        var end2 = new DateTime(2024, 1, 5);
        var dateTimeRange2 = new DateTimeOffsetRange(start2, end2);

        // Static method
        var intersection = DateTimeOffsetRange.Intersection(dateTimeRange1, dateTimeRange2);
        Assert.NotNull(intersection);
        Assert.Equal(dateTimeRange1, intersection);
        Assert.Equal(dateTimeRange2, intersection);

        // Instance method
        intersection = dateTimeRange1.Intersection(dateTimeRange2);
        Assert.NotNull(intersection);
        Assert.Equal(dateTimeRange1, intersection);
        Assert.Equal(dateTimeRange2, intersection);
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

/// <summary>
/// Represents a range of date and time values.
/// </summary>
public struct DateTimeRange : IEquatable<DateTimeRange>
{
    /// <summary>
    /// Gets or sets the start of the range.
    /// </summary>
    [JsonInclude]
    public required DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the end of the range.
    /// </summary>
    [JsonInclude]
    public required DateTime End { get; set; }

    /// <summary>
    /// Represents a range that spans over the current day.
    /// </summary>
    public static readonly DateTimeRange Today = new DateTimeRange
    {
        Start = DateTime.Today,
        End = DateTime.Today.AddDays(1)
    };

    /// <summary>
    /// Gets a value indicating whether the range is a single point in time.
    /// </summary>
    [JsonInclude]
    public bool IsTimePoint => Start == End;

    /// <summary>
    /// Gets the duration of the range.
    /// </summary>
    [JsonInclude]
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Gets the middle of the range.
    /// </summary>
    /// <remarks>
    /// If the range is a single point in time, the middle is equal to the start and end.
    /// </remarks>
    [JsonIgnore]
    public DateTime Middle => Start + Duration / 2;

    /// <summary>
    /// Gets a value indicating whether the range is valid.
    /// </summary>
    /// <remarks>
    /// A range is considered valid if the start is less than or equal to the end.
    /// </remarks>
    [JsonIgnore]
    public bool Valid => Start <= End;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <exception cref="ArgumentException">The start of the range must be less than or equal to the end of the range.</exception>
    public DateTimeRange(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException("The start of the range must be less than or equal to the end of the range.", nameof(start));
        }
        
        Start = start;
        End = end;
    }

    /// <inheritdoc/>
    public bool Equals(DateTimeRange other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeRange other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Start.ToString(CultureInfo.InvariantCulture)} - {End.ToString(CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Converts the range to a new <see cref="DateTimeRange"/> instance
    /// with the start and end values converted to universal time.
    /// </summary>
    /// <returns>A new <see cref="DateTimeRange"/> instance with the start and end values converted to universal time.</returns>
    public DateTimeRange ToUniversalTime()
    {
        return new DateTimeRange
        {
            Start = Start.ToUniversalTime(),
            End = End.ToUniversalTime()
        };
    }

    /// <summary>
    /// Converts the range to a new <see cref="DateTimeRange"/> instance
    /// with the start and end values converted to local time.
    /// </summary>
    /// <returns>A new <see cref="DateTimeRange"/> instance with the start and end values converted to local time.</returns>
    public DateTimeRange ToLocalTime()
    {
        return new DateTimeRange
        {
            Start = Start.ToLocalTime(),
            End = End.ToLocalTime()
        };
    }

    /// <summary>
    /// Adds a time span to the range.
    /// </summary>
    /// <param name="timeSpan">The time span to add.</param>
    /// <returns>A new <see cref="DateTimeRange"/> instance with the time span added to the start and end values.</returns>
    public DateTimeRange Add(TimeSpan timeSpan)
    {
        return new DateTimeRange
        {
            Start = Start.Add(timeSpan),
            End = End.Add(timeSpan)
        };
    }

    /// <summary>
    /// Determines whether the range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
    /// <returns><c>true</c> if the other range overlaps with this range; otherwise, <c>false</c>.</returns>
    public bool Overlaps(DateTimeRange other, TimeSpan minimumOverlap)
    {
        return Overlaps(this, other, minimumOverlap);
    }

    /// <summary>
    /// Determines whether the range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns><c>true</c> if the other range overlaps with this range; otherwise, <c>false</c>.</returns>
    public bool Overlaps(DateTimeRange other)
    {
        return Overlaps(this, other, TimeSpan.Zero);
    }

    /// <summary>
    /// Determines whether a <see cref="DateTime"/> value is contained within a <see cref="DateTimeRange"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
    public bool Contains(DateTime value)
    {
        return Contains(this, value);
    }

    /// <summary>
    /// Determines whether a <see cref="DateTimeRange"/> is contained within another <see cref="DateTimeRange"/>.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns><c>true</c> if the other range is contained within the range; otherwise, <c>false</c>.</returns>
    public bool Contains(DateTimeRange other)
    {
        return Contains(this, other);
    }

    /// <summary>
    /// Calculates the intersection of two <see cref="DateTimeRange"/> instances.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
    /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
    public DateTimeRange? Intersection(DateTimeRange other, TimeSpan minimumOverlap)
    {
        return Intersection(this, other, minimumOverlap);
    }

    /// <summary>
    /// Calculates the intersection of two <see cref="DateTimeRange"/> instances.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
    public DateTimeRange? Intersection(DateTimeRange other)
    {
        return Intersection(this, other, TimeSpan.Zero);
    }

    public static bool operator ==(DateTimeRange left, DateTimeRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DateTimeRange left, DateTimeRange right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines of the duration of the left range is less than the duration of the right range.
    /// </summary>
    /// <param name="left">The left range.</param>
    /// <param name="right">The right range.</param>
    /// <returns><c>true</c> if the duration of the left range is less than the duration of the right range; otherwise, <c>false</c>.</returns>
    public static bool operator <(DateTimeRange left, DateTimeRange right)
    {
        return left.Duration < right.Duration;
    }

    /// <summary>
    /// Determines of the duration of the left range is greater than the duration of the right range.
    /// </summary>
    /// <param name="left">The left range.</param>
    /// <param name="right">The right range.</param>
    /// <returns><c>true</c> if the duration of the left range is greater than the duration of the right range; otherwise, <c>false</c>.</returns>
    public static bool operator >(DateTimeRange left, DateTimeRange right)
    {
        return left.Duration > right.Duration;
    }

    /// <summary>
    /// Determines of the duration of the left range is less than or equal to the duration of the right range.
    /// </summary>
    /// <param name="left">The left range.</param>
    /// <param name="right">The right range.</param>
    /// <returns><c>true</c> if the duration of the left range is less than or equal to the duration of the right range; otherwise, <c>false</c>.</returns>
    public static bool operator <=(DateTimeRange left, DateTimeRange right)
    {
        return left.Duration <= right.Duration;
    }

    /// <summary>
    /// Determines of the duration of the left range is greater than or equal to the duration of the right range.
    /// </summary>
    /// <param name="left">The left range.</param>
    /// <param name="right">The right range.</param>
    /// <returns><c>true</c> if the duration of the left range is greater than or equal to the duration of the right range; otherwise, <c>false</c>.</returns>
    public static bool operator >=(DateTimeRange left, DateTimeRange right)
    {
        return left.Duration >= right.Duration;
    }

    /// <summary>
    /// Determines whether a <see cref="DateTime"/> value is contained within a <see cref="DateTimeRange"/>.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
    public static bool Contains(DateTimeRange range, DateTime value)
    {
        return range.Start <= value && range.End >= value;
    }

    /// <summary>
    /// Determines whether a <see cref="DateTimeRange"/> is contained within another <see cref="DateTimeRange"/>.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <param name="other">The other range.</param>
    /// <returns><c>true</c> if the other range is contained within the range; otherwise, <c>false</c>.</returns>
    public static bool Contains(DateTimeRange range, DateTimeRange other)
    {
        return range.Start <= other.Start && range.End >= other.End;
    }

    /// <summary>
    /// Determines whether two <see cref="DateTimeRange"/> instances overlap.
    /// </summary>
    /// <param name="firstRange">The first range.</param>
    /// <param name="secondRange">The second range.</param>
    /// <param name="minimumOverlap"></param>
    /// <returns><c>true</c> if the ranges overlap; otherwise, <c>false</c>.</returns>
    public static bool Overlaps(DateTimeRange firstRange, DateTimeRange secondRange, TimeSpan minimumOverlap)
    {
        // Calculate the actual overlap duration
        var overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
        var overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

        var actualOverlapDuration = overlapEnd - overlapStart;

        // Check if there is an overlap and if the overlap duration is at least the minimum required duration
        return overlapStart < overlapEnd && actualOverlapDuration >= minimumOverlap;
    }

    /// <summary>
    /// Determines whether two <see cref="DateTimeRange"/> instances overlap.
    /// </summary>
    /// <param name="firstRange">The first range.</param>
    /// <param name="secondRange">The second range.</param>
    /// <returns><c>true</c> if the ranges overlap; otherwise, <c>false</c>.</returns>
    public static bool Overlaps(DateTimeRange firstRange, DateTimeRange secondRange)
    {
        return Overlaps(firstRange, secondRange, TimeSpan.Zero);
    }

    /// <summary>
    /// Calculates the intersection of two <see cref="DateTimeRange"/> instances.
    /// </summary>
    /// <param name="firstRange">The first range.</param>
    /// <param name="secondRange">The second range.</param>
    /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
    /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
    public static DateTimeRange? Intersection(DateTimeRange firstRange, DateTimeRange secondRange, TimeSpan minimumOverlap)
    {
        if (!Overlaps(firstRange, secondRange, minimumOverlap))
        {
            return null;
        }

        var overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
        var overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

        return new DateTimeRange
        {
            Start = overlapStart,
            End = overlapEnd
        };
    }

    /// <summary>
    /// Calculates the intersection of two <see cref="DateTimeRange"/> instances.
    /// </summary>
    /// <param name="firstRange">The first range.</param>
    /// <param name="secondRange">The second range.</param>
    /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
    public static DateTimeRange? Intersection(DateTimeRange firstRange, DateTimeRange secondRange)
    {
        return Intersection(firstRange, secondRange, TimeSpan.Zero);
    }
}
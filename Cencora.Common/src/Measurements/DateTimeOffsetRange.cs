// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements
{
    /// <summary>
    /// Represents a range of date and time values.
    /// </summary>
    public struct DateTimeOffsetRange : IEquatable<DateTimeOffsetRange>
    {
        /// <summary>
        /// Gets or sets the start of the range.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// Gets or sets the end of the range.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset End { get; set; }

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
        public DateTimeOffset Middle => Start + Duration / 2;

        /// <summary>
        /// Gets a value indicating whether the range is valid.
        /// </summary>
        /// <remarks>
        /// A range is considered valid if the start is less than or equal to the end.
        /// </remarks>
        [JsonIgnore]
        public bool Valid => Start <= End;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> struct.
        /// </summary>
        public DateTimeOffsetRange()
        {
            Start = DateTimeOffset.MinValue;
            End = DateTimeOffset.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> struct.
        /// </summary>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }

        /// <inheritdoc/>
        public bool Equals(DateTimeOffsetRange other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is DateTimeOffsetRange other && Equals(other);
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
        /// Converts the range to a new <see cref="DateTimeOffsetRange"/> instance
        /// with the start and end values converted to universal time.
        /// </summary>
        /// <returns>A new <see cref="DateTimeOffsetRange"/> instance with the start and end values converted to universal time.</returns>
        public DateTimeOffsetRange ToUniversalTime()
        {
            return new DateTimeOffsetRange
            {
                Start = Start.ToUniversalTime(),
                End = End.ToUniversalTime()
            };
        }

        /// <summary>
        /// Converts the range to a new <see cref="DateTimeOffsetRange"/> instance
        /// with the start and end values converted to local time.
        /// </summary>
        /// <returns>A new <see cref="DateTimeOffsetRange"/> instance with the start and end values converted to local time.</returns>
        public DateTimeOffsetRange ToLocalTime()
        {
            return new DateTimeOffsetRange
            {
                Start = Start.ToLocalTime(),
                End = End.ToLocalTime()
            };
        }

        /// <summary>
        /// Adds a time span to the range.
        /// </summary>
        /// <param name="timeSpan">The time span to add.</param>
        /// <returns>A new <see cref="DateTimeOffsetRange"/> instance with the time span added to the start and end values.</returns>
        public DateTimeOffsetRange Add(TimeSpan timeSpan)
        {
            return new DateTimeOffsetRange
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
        public bool Overlaps(DateTimeOffsetRange other, TimeSpan minimumOverlap)
        {
            return Overlaps(this, other, minimumOverlap);
        }

        /// <summary>
        /// Determines whether the range overlaps with another range.
        /// </summary>
        /// <param name="other">The other range.</param>
        /// <returns><c>true</c> if the other range overlaps with this range; otherwise, <c>false</c>.</returns>
        public bool Overlaps(DateTimeOffsetRange other)
        {
            return Overlaps(this, other, TimeSpan.Zero);
        }

        /// <summary>
        /// Determines whether a <see cref="DateTimeOffset"/> value is contained within a <see cref="DateTimeOffsetRange"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
        public bool Contains(DateTimeOffset value)
        {
            return Contains(this, value);
        }

        /// <summary>
        /// Determines whether a <see cref="DateTimeOffsetRange"/> is contained within another <see cref="DateTimeOffsetRange"/>.
        /// </summary>
        /// <param name="other">The other range.</param>
        /// <returns><c>true</c> if the other range is contained within the range; otherwise, <c>false</c>.</returns>
        public bool Contains(DateTimeOffsetRange other)
        {
            return Contains(this, other);
        }

        /// <summary>
        /// Calculates the intersection of two <see cref="DateTimeOffsetRange"/> instances.
        /// </summary>
        /// <param name="other">The other range.</param>
        /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
        /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
        public DateTimeOffsetRange? Intersection(DateTimeOffsetRange other, TimeSpan minimumOverlap)
        {
            return Intersection(this, other, minimumOverlap);
        }

        /// <summary>
        /// Calculates the intersection of two <see cref="DateTimeOffsetRange"/> instances.
        /// </summary>
        /// <param name="other">The other range.</param>
        /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
        public DateTimeOffsetRange? Intersection(DateTimeOffsetRange other)
        {
            return Intersection(this, other, TimeSpan.Zero);
        }

        public static bool operator ==(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines of the duration of the left range is less than the duration of the right range.
        /// </summary>
        /// <param name="left">The left range.</param>
        /// <param name="right">The right range.</param>
        /// <returns><c>true</c> if the duration of the left range is less than the duration of the right range; otherwise, <c>false</c>.</returns>
        public static bool operator <(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return left.Duration < right.Duration;
        }

        /// <summary>
        /// Determines of the duration of the left range is greater than the duration of the right range.
        /// </summary>
        /// <param name="left">The left range.</param>
        /// <param name="right">The right range.</param>
        /// <returns><c>true</c> if the duration of the left range is greater than the duration of the right range; otherwise, <c>false</c>.</returns>
        public static bool operator >(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return left.Duration > right.Duration;
        }

        /// <summary>
        /// Determines of the duration of the left range is less than or equal to the duration of the right range.
        /// </summary>
        /// <param name="left">The left range.</param>
        /// <param name="right">The right range.</param>
        /// <returns><c>true</c> if the duration of the left range is less than or equal to the duration of the right range; otherwise, <c>false</c>.</returns>
        public static bool operator <=(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return left.Duration <= right.Duration;
        }

        /// <summary>
        /// Determines of the duration of the left range is greater than or equal to the duration of the right range.
        /// </summary>
        /// <param name="left">The left range.</param>
        /// <param name="right">The right range.</param>
        /// <returns><c>true</c> if the duration of the left range is greater than or equal to the duration of the right range; otherwise, <c>false</c>.</returns>
        public static bool operator >=(DateTimeOffsetRange left, DateTimeOffsetRange right)
        {
            return left.Duration >= right.Duration;
        }

        /// <summary>
        /// Determines whether a <see cref="DateTimeOffset"/> value is contained within a <see cref="DateTimeOffsetRange"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
        public static bool Contains(DateTimeOffsetRange range, DateTimeOffset value)
        {
            return range.Start <= value && range.End >= value;
        }

        /// <summary>
        /// Determines whether a <see cref="DateTimeOffsetRange"/> is contained within another <see cref="DateTimeOffsetRange"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="other">The other range.</param>
        /// <returns><c>true</c> if the other range is contained within the range; otherwise, <c>false</c>.</returns>
        public static bool Contains(DateTimeOffsetRange range, DateTimeOffsetRange other)
        {
            return range.Start <= other.Start && range.End >= other.End;
        }

        /// <summary>
        /// Determines whether two <see cref="DateTimeOffsetRange"/> instances overlap.
        /// </summary>
        /// <param name="firstRange">The first range.</param>
        /// <param name="secondRange">The second range.</param>
        /// <param name="minimumOverlap"></param>
        /// <returns><c>true</c> if the ranges overlap; otherwise, <c>false</c>.</returns>
        public static bool Overlaps(DateTimeOffsetRange firstRange, DateTimeOffsetRange secondRange, TimeSpan minimumOverlap)
        {
            // Calculate the actual overlap duration
            DateTimeOffset overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
            DateTimeOffset overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

            TimeSpan actualOverlapDuration = overlapEnd - overlapStart;

            // Check if there is an overlap and if the overlap duration is at least the minimum required duration
            return overlapStart < overlapEnd && actualOverlapDuration >= minimumOverlap;
        }

        /// <summary>
        /// Determines whether two <see cref="DateTimeOffsetRange"/> instances overlap.
        /// </summary>
        /// <param name="firstRange">The first range.</param>
        /// <param name="secondRange">The second range.</param>
        /// <returns><c>true</c> if the ranges overlap; otherwise, <c>false</c>.</returns>
        public static bool Overlaps(DateTimeOffsetRange firstRange, DateTimeOffsetRange secondRange)
        {
            return Overlaps(firstRange, secondRange, TimeSpan.Zero);
        }

        /// <summary>
        /// Calculates the intersection of two <see cref="DateTimeOffsetRange"/> instances.
        /// </summary>
        /// <param name="firstRange">The first range.</param>
        /// <param name="secondRange">The second range.</param>
        /// <param name="minimumOverlap">The minimum overlap required for the ranges to be considered overlapping.</param>
        /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
        public static DateTimeOffsetRange? Intersection(DateTimeOffsetRange firstRange, DateTimeOffsetRange secondRange, TimeSpan minimumOverlap)
        {
            if (!Overlaps(firstRange, secondRange, minimumOverlap))
            {
                return null;
            }

            DateTimeOffset overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
            DateTimeOffset overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

            return new DateTimeOffsetRange
            {
                Start = overlapStart,
                End = overlapEnd
            };
        }

        /// <summary>
        /// Calculates the intersection of two <see cref="DateTimeOffsetRange"/> instances.
        /// </summary>
        /// <param name="firstRange">The first range.</param>
        /// <param name="secondRange">The second range.</param>
        /// <returns>The intersection of the two ranges, or <c>null</c> if the ranges do not overlap.</returns>
        public static DateTimeOffsetRange? Intersection(DateTimeOffsetRange firstRange, DateTimeOffsetRange secondRange)
        {
            return Intersection(firstRange, secondRange, TimeSpan.Zero);
        }
    }
}
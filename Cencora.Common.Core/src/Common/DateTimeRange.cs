// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a range of date and time values.
    /// </summary>
    public struct DateTimeRange : IEquatable<DateTimeRange>
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

        public static readonly DateTimeRange Default = new DateTimeRange { Start = DateTimeOffset.MinValue, End = DateTimeOffset.MaxValue };

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        public DateTimeRange()
        {
            Start = DateTimeOffset.MinValue;
            End = DateTimeOffset.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <exception cref="ArgumentException">Thrown when the start is after the end.</exception>
        public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            if (start > end)
            {
                throw new ArgumentException("The start of the range must be before the end.");
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
        /// Determines whether a <see cref="DateTimeOffset"/> value is contained within a <see cref="DateTimeRange"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
        public bool Contains(DateTimeOffset value)
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
        /// Determines whether a <see cref="DateTimeOffset"/> value is contained within a <see cref="DateTimeRange"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value is contained within the range; otherwise, <c>false</c>.</returns>
        public static bool Contains(DateTimeRange range, DateTimeOffset value)
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
        /// <returns><c>true</c> if the ranges overlap; otherwise, <c>false</c>.</returns>
        public static bool Overlaps(DateTimeRange firstRange, DateTimeRange secondRange, TimeSpan minimumOverlap)
        {
            // Calculate the actual overlap duration
            DateTimeOffset overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
            DateTimeOffset overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

            TimeSpan actualOverlapDuration = overlapEnd - overlapStart;

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

            DateTimeOffset overlapStart = firstRange.Start > secondRange.Start ? firstRange.Start : secondRange.Start;
            DateTimeOffset overlapEnd = firstRange.End < secondRange.End ? firstRange.End : secondRange.End;

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
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

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
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end of the range.
        /// </summary>
        [JsonInclude]
        public DateTime End { get; set; }

        /// <summary>
        /// Gets a value indicating whether the range is a single point in time.
        /// </summary>
        [JsonInclude]
        public bool IsTimePoint => Start == End;

        /// <summary>
        /// Gets the duration of the range.
        /// </summary>
        [JsonIgnore]
        public TimeSpan Duration => End - Start;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        public DateTimeRange()
        {
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
        /// </summary>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <exception cref="ArgumentException">Thrown when the start is after the end.</exception>
        public DateTimeRange(DateTime start, DateTime end)
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

        public static bool operator ==(DateTimeRange left, DateTimeRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateTimeRange left, DateTimeRange right)
        {
            return !left.Equals(right);
        }
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a range of temperatures.
    /// </summary>
    public struct TemperatureRange : IEquatable<TemperatureRange>
    {
        /// <summary>
        /// Gets or sets the minimum temperature of the range.
        /// </summary>
        [JsonInclude]
        public required Temperature Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum temperature of the range.
        /// </summary>
        [JsonInclude]
        public required Temperature Max { get; set; }

        /// <summary>
        /// Gets a value indicating whether the range is a single temperature.
        /// </summary>
        [JsonIgnore]
        public bool IsSingleTemperature => Min == Max;

        /// <inheritdoc/>
        public bool Equals(TemperatureRange other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is TemperatureRange other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        public static bool operator ==(TemperatureRange left, TemperatureRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TemperatureRange left, TemperatureRange right)
        {
            return !left.Equals(right);
        }
    }
}
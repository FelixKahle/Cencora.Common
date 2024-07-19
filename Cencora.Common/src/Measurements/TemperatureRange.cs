// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

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

    /// <summary>
    /// The default temperature range.
    /// Ranges from <see cref="Temperature.MinValue"/> to <see cref="Temperature.MaxValue"/>.
    /// </summary>
    public static readonly TemperatureRange Default = new() { Min = Temperature.MinValue, Max = Temperature.MaxValue };

    /// <summary>
    /// Initializes a new instance of the <see cref="TemperatureRange"/> struct.
    /// </summary>
    /// <param name="min">The minimum temperature of the range.</param>
    /// <param name="max">The maximum temperature of the range.</param>
    /// <exception cref="ArgumentException">The minimum temperature must be less than or equal to the maximum temperature.</exception>
    public TemperatureRange(Temperature min, Temperature max)
    {
        if (min > max)
        {
            throw new ArgumentException("The minimum temperature must be less than or equal to the maximum temperature.", nameof(min));
        }

        Min = min;
        Max = max;
    }

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
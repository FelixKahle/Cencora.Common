// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

/// <summary>
/// Represents the unit of distance.
/// </summary>
public enum DistanceUnit
{
    Millimeter,
    Centimeter,
    Meter,
    Kilometer,
    Inch,
    Foot,
    Yard,
    Mile,
    NauticalMile
}

/// <summary>
/// Provides extension methods for the <see cref="DistanceUnit"/> enum.
/// </summary>
public static class DistanceUnitExtensions
{
    /// <summary>
    /// The valid distance units.
    /// </summary>
    private static readonly HashSet<string> ValidUnits =
    [
        "millimeters", "millimeter", "mm",
        "centimeters", "centimeter", "cm",
        "meters", "meter", "m",
        "kilometers", "kilometer", "km",
        "inches", "inch", "in",
        "feet", "foot", "ft",
        "yards", "yard", "yd",
        "miles", "mile", "mi",
        "nauticalmiles", "nauticalmile", "nmi"
    ];

    /// <summary>
    /// Converts the distance unit to its corresponding string representation.
    /// </summary>
    /// <param name="unit">The distance unit to convert.</param>
    /// <returns>The string representation of the distance unit.</returns>
    public static string ToUnitString(this DistanceUnit unit)
    {
        return unit switch
        {
            DistanceUnit.Millimeter => "mm",
            DistanceUnit.Centimeter => "cm",
            DistanceUnit.Meter => "m",
            DistanceUnit.Kilometer => "km",
            DistanceUnit.Inch => "in",
            DistanceUnit.Foot => "ft",
            DistanceUnit.Yard => "yd",
            DistanceUnit.Mile => "mi",
            DistanceUnit.NauticalMile => "nmi",
            _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
        };
    }

    /// <summary>
    /// Converts a string representation to the corresponding <see cref="DistanceUnit"/> value.
    /// </summary>
    /// <param name="unit">The string representation of the distance unit.</param>
    /// <returns>The corresponding <see cref="DistanceUnit"/> value.</returns>
    public static DistanceUnit FromString(string unit)
    {
        return unit.ToLower().Replace(" ", "").Trim() switch
        {
            "millimeters" or "millimeter" or "mm" => DistanceUnit.Millimeter,
            "centimeters" or "centimeter" or "cm" => DistanceUnit.Centimeter,
            "meters" or "meter" or "m" => DistanceUnit.Meter,
            "kilometers" or "kilometer" or "km" => DistanceUnit.Kilometer,
            "inches" or "inch" or "in" => DistanceUnit.Inch,
            "feet" or "foot" or "ft" => DistanceUnit.Foot,
            "yards" or "yard" or "yd" => DistanceUnit.Yard,
            "miles" or "mile" or "mi" => DistanceUnit.Mile,
            "nauticalmiles" or "nauticalmile" or "nmi" => DistanceUnit.NauticalMile,
            _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
        };
    }

    /// <summary>
    /// Checks if the specified string is a valid distance unit format.
    /// </summary>
    /// <param name="unit">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid distance unit format; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The string is normalized by removing spaces and converting to lowercase before checking.
    /// </remarks>
    public static bool IsValidUnitString(string unit)
    {
        return ValidUnits.Contains(unit.ToLower().Replace(" ", "").Trim());
    }
}

/// <summary>
/// Represents a distance measurement.
/// </summary>
/// <remarks>
/// The <see cref="Distance"/> struct provides functionality to work with distances in various units, such as meters, kilometers, inches, feet, etc.
/// It allows conversion between different units and comparison of distances.
/// Negative values are not allowed for distances and will result in an exception.
/// </remarks>
[JsonConverter(typeof(DistanceConverter))]
public struct Distance : IEquatable<Distance>, IFormattable, IComparable<Distance>, IComparable
{
    /// <summary>
    /// Represents the distance in meters.
    /// </summary>
    private double _meters;

    public static readonly Distance Zero = new(0, DistanceUnit.Meter);
    public static readonly Distance MinValue = new(0, DistanceUnit.Meter);
    public static readonly Distance MaxValue = new(double.MaxValue, DistanceUnit.Meter);

    public static Distance FromMillimeters(double value) => new(value, DistanceUnit.Millimeter);
    public static Distance FromCentimeters(double value) => new(value, DistanceUnit.Centimeter);
    public static Distance FromMeters(double value) => new(value, DistanceUnit.Meter);
    public static Distance FromKilometers(double value) => new(value, DistanceUnit.Kilometer);
    public static Distance FromInches(double value) => new(value, DistanceUnit.Inch);
    public static Distance FromFeet(double value) => new(value, DistanceUnit.Foot);
    public static Distance FromYards(double value) => new(value, DistanceUnit.Yard);
    public static Distance FromMiles(double value) => new(value, DistanceUnit.Mile);
    public static Distance FromNauticalMiles(double value) => new(value, DistanceUnit.NauticalMile);

    /// <summary>
    /// Initializes a new instance of the <see cref="Distance"/> struct with a default value of 0 meters.
    /// </summary>
    public Distance()
    {
        _meters = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Distance"/> struct with the specified value and unit.
    /// </summary>
    /// <param name="value">The value of the distance.</param>
    /// <param name="unit">The unit of the distance.</param>
    /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
    public Distance(double value, DistanceUnit unit)
    {
        _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), unit);
    }

    /// <summary>
    /// Gets or sets the distance value in millimeters.
    /// </summary>
    public double Millimeters
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Millimeter);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Millimeter);
    }

    /// <summary>
    /// Gets or sets the distance value in centimeters.
    /// </summary>
    public double Centimeters
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Centimeter);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Centimeter);
    }

    /// <summary>
    /// Gets or sets the distance value in meters.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is negative.</exception>
    public double Meters
    {
        get => _meters;
        init => _meters = Math.Clamp(value, 0, double.MaxValue);
    }

    /// <summary>
    /// Gets or sets the distance value in kilometers.
    /// </summary>
    public double Kilometers
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Kilometer);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Kilometer);
    }

    /// <summary>
    /// Gets or sets the distance value in inches.
    /// </summary>
    public double Inches
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Inch);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Inch);
    }

    /// <summary>
    /// Gets or sets the distance value in feet.
    /// </summary>
    public double Feet
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Foot);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Foot);
    }

    /// <summary>
    /// Gets or sets the distance value in yards.
    /// </summary>
    public double Yards
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Yard);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Yard);
    }

    /// <summary>
    /// Gets or sets the distance value in miles.
    /// </summary>
    public double Miles
    {
        get => ConvertFromMeters(_meters, DistanceUnit.Mile);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.Mile);
    }

    /// <summary>
    /// Gets or sets the distance value in nautical miles.
    /// </summary>
    public double NauticalMiles
    {
        get => ConvertFromMeters(_meters, DistanceUnit.NauticalMile);
        set => _meters = ConvertToMeters(Math.Clamp(value, 0, double.MaxValue), DistanceUnit.NauticalMile);
    }

    /// <summary>
    /// Converts the specified value from the given distance unit to meters.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <param name="unit">The distance unit of the value.</param>
    /// <returns>The converted value in meters.</returns>
    /// <exception cref="ArgumentException">Thrown when the distance unit is invalid.</exception>
    private static double ConvertToMeters(double value, DistanceUnit unit)
    {
        return unit switch
        {
            DistanceUnit.Millimeter => value / 1000,
            DistanceUnit.Centimeter => value / 100,
            DistanceUnit.Meter => value,
            DistanceUnit.Kilometer => value * 1000,
            DistanceUnit.Inch => value * 0.0254,
            DistanceUnit.Foot => value * 0.3048,
            DistanceUnit.Yard => value * 0.9144,
            DistanceUnit.Mile => value * 1609.34,
            DistanceUnit.NauticalMile => value * 1852,
            _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
        };
    }

    /// <summary>
    /// Converts the specified value from meters to the given distance unit.
    /// </summary>
    /// <param name="meters">The value to be converted.</param>
    /// <param name="unit">The distance unit to convert the value to.</param>
    /// <returns>The converted value in the specified distance unit.</returns>
    /// <exception cref="ArgumentException">Thrown when the distance unit is invalid.</exception>
    private static double ConvertFromMeters(double meters, DistanceUnit unit)
    {
        return unit switch
        {
            DistanceUnit.Millimeter => meters * 1000,
            DistanceUnit.Centimeter => meters * 100,
            DistanceUnit.Meter => meters,
            DistanceUnit.Kilometer => meters / 1000,
            DistanceUnit.Inch => meters / 0.0254,
            DistanceUnit.Foot => meters / 0.3048,
            DistanceUnit.Yard => meters / 0.9144,
            DistanceUnit.Mile => meters / 1609.34,
            DistanceUnit.NauticalMile => meters / 1852,
            _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
        };
    }

    /// <inheritdoc/>
    public bool Equals(Distance other)
    {
        return Meters.Equals(other.Meters);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Distance other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Meters.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString("m", CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        format = string.IsNullOrEmpty(format) ? "m" : format;
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        if (!DistanceUnitExtensions.IsValidUnitString(format))
        {
            throw new FormatException("Invalid format string");
        }

        var unit = DistanceUnitExtensions.FromString(format);
        var value = ConvertFromMeters(Meters, unit);

        return $"{value.ToString(provider)} {unit.ToUnitString()}";
    }

    /// <inheritdoc/>
    public int CompareTo(Distance other)
    {
        return Meters.CompareTo(other.Meters);
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is Distance distance)
        {
            return CompareTo(distance);
        }

        throw new ArgumentException("Object is not a Distance", nameof(obj));
    }

    /// <summary>
    /// Compares two <see cref="Distance"/> instances for equality.
    /// </summary>
    /// <param name="left">The first <see cref="Distance"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Distance"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Distance left, Distance right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="Distance"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first <see cref="Distance"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Distance"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Distance left, Distance right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(Distance left, Distance right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(Distance left, Distance right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(Distance left, Distance right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(Distance left, Distance right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static Distance operator +(Distance left, Distance right)
    {
        var value = Math.Clamp(left.Meters + right.Meters, 0, double.MaxValue);
        return new Distance(value, DistanceUnit.Meter);
    }

    public static Distance operator -(Distance left, Distance right)
    {
        var value = Math.Clamp(left.Meters - right.Meters, 0, double.MaxValue);
        return new Distance(value, DistanceUnit.Meter);
    }
}

/// <summary>
/// Provides a custom JSON converter for the <see cref="Distance"/> struct.
/// Saves the distance value in meters.
/// </summary>
public class DistanceConverter : JsonConverter<Distance>
{
    /// <inheritdoc/>
    public override Distance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        double value = 0;
        var unit = DistanceUnit.Meter;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Distance(value, unit);
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            var propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

            if (propertyName == valuePropertyName)
            {
                reader.Read();
                value = reader.GetDouble();
            }
            else if (propertyName == unitPropertyName)
            {
                reader.Read();
                unit = DistanceUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
            }
            else
            {
                throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Unexpected end of JSON");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Distance value, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

        writer.WriteStartObject();
        writer.WriteNumber(valuePropertyName, value.Meters);
        writer.WriteString(unitPropertyName, DistanceUnit.Meter.ToUnitString());
        writer.WriteEndObject();
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents the unit of distance.
    /// </summary>
    public enum DistanceUnit
    {
        Millimeters,
        Centimeters,
        Meters,
        Kilometers,
        Inches,
        Feet,
        Yards,
        Miles,
        NauticalMiles
    }

    /// <summary>
    /// Provides extension methods for the <see cref="DistanceUnit"/> enum.
    /// </summary>
    public static class DistanceUnitExtensions
    {
        /// <summary>
        /// Converts the distance unit to its corresponding string representation.
        /// </summary>
        /// <param name="unit">The distance unit to convert.</param>
        /// <returns>The string representation of the distance unit.</returns>
        public static string ToUnitString(this DistanceUnit unit)
        {
            return unit switch
            {
                DistanceUnit.Millimeters => "mm",
                DistanceUnit.Centimeters => "cm",
                DistanceUnit.Meters => "m",
                DistanceUnit.Kilometers => "km",
                DistanceUnit.Inches => "in",
                DistanceUnit.Feet => "ft",
                DistanceUnit.Yards => "yd",
                DistanceUnit.Miles => "mi",
                DistanceUnit.NauticalMiles => "nmi",
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
            return unit.ToLower().Trim() switch
            {
                "millimeters" or "millimeter" or "mm" => DistanceUnit.Millimeters,
                "centimeters" or "centimeter" or "cm" => DistanceUnit.Centimeters,
                "meters" or "meter" or "m" => DistanceUnit.Meters,
                "kilometers" or "kilometer" or "km" => DistanceUnit.Kilometers,
                "inches" or "inch" or "in" => DistanceUnit.Inches,
                "feet" or "foot" or "ft" => DistanceUnit.Feet,
                "yards" or "yard" or "yd" => DistanceUnit.Yards,
                "miles" or "mile" or "mi" => DistanceUnit.Miles,
                "nautical miles" or "nautical mile" or "nmi" => DistanceUnit.NauticalMiles,
                _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
            };
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
            if (value < 0)
            {
                throw new ArgumentException("Distance value cannot be negative", nameof(value));
            }

            _meters = ConvertToMeters(value, unit);
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
                DistanceUnit.Millimeters => value / 1000,
                DistanceUnit.Centimeters => value / 100,
                DistanceUnit.Meters => value,
                DistanceUnit.Kilometers => value * 1000,
                DistanceUnit.Inches => value * 0.0254,
                DistanceUnit.Feet => value * 0.3048,
                DistanceUnit.Yards => value * 0.9144,
                DistanceUnit.Miles => value * 1609.34,
                DistanceUnit.NauticalMiles => value * 1852,
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
                DistanceUnit.Millimeters => meters * 1000,
                DistanceUnit.Centimeters => meters * 100,
                DistanceUnit.Meters => meters,
                DistanceUnit.Kilometers => meters / 1000,
                DistanceUnit.Inches => meters / 0.0254,
                DistanceUnit.Feet => meters / 0.3048,
                DistanceUnit.Yards => meters / 0.9144,
                DistanceUnit.Miles => meters / 1609.34,
                DistanceUnit.NauticalMiles => meters / 1852,
                _ => throw new ArgumentException("Invalid distance unit", nameof(unit))
            };
        }

        /// <inheritdoc/>
        public bool Equals(Distance other)
        {
            return _meters.Equals(other._meters);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Distance other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _meters.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString("m", CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            format = string.IsNullOrEmpty(format) ? "m" : format;
            IFormatProvider provider = formatProvider ?? CultureInfo.InvariantCulture;

            DistanceUnit unit = DistanceUnitExtensions.FromString(format);
            double value = ConvertFromMeters(_meters, unit);
            
            return $"{value.ToString(provider)} {unit.ToUnitString()}";
        }

        /// <inheritdoc/>
        public int CompareTo(Distance other)
        {
            return _meters.CompareTo(other._meters);
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
            return new Distance(left._meters + right._meters, DistanceUnit.Meters);
        }

        public static Distance operator -(Distance left, Distance right)
        {
            return new Distance(left._meters - right._meters, DistanceUnit.Meters);
        }

        public static Distance operator *(Distance distance, double factor)
        {
            return new Distance(distance._meters * factor, DistanceUnit.Meters);
        }

        public static Distance operator *(double factor, Distance distance)
        {
            return distance * factor;
        }

        public static Distance operator *(Distance left, Distance right)
        {
            return new Distance(left._meters * right._meters, DistanceUnit.Meters);
        }

        public static Distance operator /(Distance distance, double divisor)
        {
            return new Distance(distance._meters / divisor, DistanceUnit.Meters);
        }

        public static Distance operator /(double dividend, Distance distance)
        {
            return new Distance(dividend / distance._meters, DistanceUnit.Meters);
        }

        public static double operator /(Distance left, Distance right)
        {
            return left._meters / right._meters;
        }

        public double Millimeters
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Millimeters);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Millimeters);
            }
        }

        public double Centimeters
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Centimeters);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Centimeters);
            }
        }

        public double Meters
        {
            get => _meters;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = value;
            }
        }

        public double Kilometers
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Kilometers);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Kilometers);
            }
        }

        public double Inches
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Inches);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Inches);
            }
        }

        public double Feet
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Feet);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Feet);
            }
        }

        public double Yards
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Yards);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Yards);
            }
        }

        public double Miles
        {
            get => ConvertFromMeters(_meters, DistanceUnit.Miles);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.Miles);
            }
        }

        public double NauticalMiles
        {
            get => ConvertFromMeters(_meters, DistanceUnit.NauticalMiles);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance value cannot be negative", nameof(value));
                }

                _meters = ConvertToMeters(value, DistanceUnit.NauticalMiles);
            }
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
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            double value = 0;
            DistanceUnit unit = DistanceUnit.Meters;

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

                string propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

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
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            writer.WriteStartObject();
            writer.WriteNumber(valuePropertyName, value.Meters);
            writer.WriteString(unitPropertyName, DistanceUnit.Meters.ToUnitString());
            writer.WriteEndObject();
        }
    }
}
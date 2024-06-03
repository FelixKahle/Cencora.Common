// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a volume unit.
    /// </summary>
    public enum VolumeUnit
    {
        CubicCentimeter,
        CubicMeter,
        CubicFoot,
        Liter,
        Gallon
    }

    /// <summary>
    /// Provides extension methods for the <see cref="VolumeUnit"/> enum.
    /// </summary>
    public static class VolumeUnitExtensions
    {
        /// <summary>
        /// Converts the <see cref="VolumeUnit"/> to its string representation.
        /// </summary>
        /// <param name="unit">The volume unit to convert.</param>
        /// <returns>The string representation of the volume unit.</returns>
        /// <exception cref="ArgumentException">Thrown when the volume unit is invalid.</exception>
        public static string ToUnitString(this VolumeUnit unit) => unit switch
        {
            VolumeUnit.CubicCentimeter => "cm³",
            VolumeUnit.CubicMeter => "m³",
            VolumeUnit.CubicFoot => "ft³",
            VolumeUnit.Liter => "l",
            VolumeUnit.Gallon => "gal",
            _ => throw new ArgumentException("Invalid volume unit", nameof(unit))
        };

        /// <summary>
        /// Converts the string representation of a volume unit to the corresponding <see cref="VolumeUnit"/> value.
        /// </summary>
        /// <param name="unit">The string representation of the volume unit.</param>
        /// <returns>The <see cref="VolumeUnit"/> value corresponding to the string representation.</returns>
        /// <exception cref="ArgumentException">Thrown when the string representation is invalid.</exception>
        public static VolumeUnit FromString(string unit)
        {
            return unit.ToLower().Replace(" ", "").Trim() switch
            {
                "cm³" or "cm3" or "cubiccentimeter" => VolumeUnit.CubicCentimeter,
                "m³" or "m3" or "cubicmeter" => VolumeUnit.CubicMeter,
                "ft³" or "ft3" or "cubicfoot" => VolumeUnit.CubicFoot,
                "l" or "liter" => VolumeUnit.Liter,
                "gal" or "gallon" => VolumeUnit.Gallon,
                _ => throw new ArgumentException("Invalid volume unit", nameof(unit))
            };
        }
    }

    /// <summary>
    /// Represents a volume.
    /// </summary>
    [JsonConverter(typeof(VolumeConverter))]
    public struct Volume : IEquatable<Volume>, IComparable<Volume>, IComparable, IFormattable
    {
        /// <summary>
        /// The value of the volume in cubic meters.
        /// </summary>
        private double _cubicMeters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Volume"/> class.
        /// </summary>
        /// <param name="value">The value of the volume.</param>
        /// <param name="unit">The unit of measurement for the volume.</param>
        public Volume(double value, VolumeUnit unit)
        {
            _cubicMeters = ToCubicMeters(value, unit);
        }

        /// <summary>
        /// Converts the specified value from the given volume unit to cubic meters.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="unit">The volume unit of the value.</param>
        /// <returns>The converted value in cubic meters.</returns>
        /// <exception cref="ArgumentException">Thrown when the volume unit is invalid.</exception>
        private static double ToCubicMeters(double value, VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.CubicCentimeter => value * 0.000001,
                VolumeUnit.CubicMeter => value,
                VolumeUnit.CubicFoot => value * 0.0283168,
                VolumeUnit.Liter => value * 0.001,
                VolumeUnit.Gallon => value * 0.00378541,
                _ => throw new ArgumentException("Invalid volume unit", nameof(unit))
            };
        }

        /// <summary>
        /// Converts a volume value from cubic meters to the specified unit.
        /// </summary>
        /// <param name="value">The volume value in cubic meters.</param>
        /// <param name="unit">The unit to convert the volume to.</param>
        /// <returns>The converted volume value.</returns>
        private static double FromCubicMeters(double value, VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.CubicCentimeter => value / 0.000001,
                VolumeUnit.CubicMeter => value,
                VolumeUnit.CubicFoot => value / 0.0283168,
                VolumeUnit.Liter => value / 0.001,
                VolumeUnit.Gallon => value / 0.00378541,
                _ => throw new ArgumentException("Invalid volume unit", nameof(unit))
            };
        }

        /// <inheritdoc/>
        public bool Equals(Volume other)
        {
            return _cubicMeters.Equals(other._cubicMeters);
        }

        /// <inheritdoc/>
        public int CompareTo(Volume other)
        {
            return _cubicMeters.CompareTo(other._cubicMeters);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Volume other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _cubicMeters.GetHashCode();
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Volume other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException("Object is not a Volume", nameof(obj));
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            format = string.IsNullOrEmpty(format) ? "m3" : format;
            IFormatProvider provider = formatProvider ?? CultureInfo.InvariantCulture;

            VolumeUnit unit = VolumeUnitExtensions.FromString(format);
            double value = FromCubicMeters(_cubicMeters, unit);
            
            return $"{value.ToString(provider)} {unit.ToUnitString()}";
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString("m3", CultureInfo.InvariantCulture);
        }

        public double CubicCentimeters
        {
            get => _cubicMeters / 0.000001;
            set => _cubicMeters = value * 0.000001;
        }

        public double CubicMeters
        {
            get => _cubicMeters;
            set => _cubicMeters = value;
        }

        public double CubicFeet 
        {
            get => _cubicMeters / 0.0283168;
            set => _cubicMeters = value * 0.0283168;
        }

        public double Liters
        {
            get => _cubicMeters / 0.001;
            set => _cubicMeters = value * 0.001;
        }

        public double Gallons
        {
            get => _cubicMeters / 0.00378541;
            set => _cubicMeters = value * 0.00378541;
        }

        public static bool operator ==(Volume left, Volume right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Volume left, Volume right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Volume left, Volume right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Volume left, Volume right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Volume left, Volume right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Volume left, Volume right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static Volume operator +(Volume left, Volume right)
        {
            return new Volume(left._cubicMeters + right._cubicMeters, VolumeUnit.CubicMeter);
        }

        public static Volume operator -(Volume left, Volume right)
        {
            return new Volume(left._cubicMeters - right._cubicMeters, VolumeUnit.CubicMeter);
        }
    }

    /// <summary>
    /// Converts a <see cref="Volume"/> object to and from JSON.
    /// </summary>
    public class VolumeConverter : JsonConverter<Volume>
    {
        /// <inheritdoc/>
        public override Volume Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            double value = 0;
            VolumeUnit unit = VolumeUnit.CubicMeter;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Volume(value, unit);
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
                    unit = VolumeUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
                }
                else
                {
                    throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Volume value, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            writer.WriteStartObject();
            writer.WriteNumber(valuePropertyName, value.CubicMeters);
            writer.WriteString(unitPropertyName, VolumeUnitExtensions.ToUnitString(VolumeUnit.CubicMeter));
            writer.WriteEndObject();
        }
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents the unit of temperature.
    /// </summary>
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    /// <summary>
    /// Provides extension methods for working with temperature units.
    /// </summary>
    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// The set of valid temperature units.
        /// </summary>
        private static readonly HashSet<string> ValidUnits = new HashSet<string> 
        { 
            "c", "celsius", "f", 
            "fahrenheit", "k", 
            "kelvin" 
        };

        /// <summary>
        /// Converts a string representation of a temperature unit to the corresponding <see cref="TemperatureUnit"/> enum value.
        /// </summary>
        /// <param name="unit">The string representation of the temperature unit.</param>
        /// <returns>The <see cref="TemperatureUnit"/> enum value corresponding to the input string.</returns>
        public static TemperatureUnit FromString(string unit)
        {
            return unit.Replace("°", "").ToLower().Trim() switch
            {
                "c" or "celsius" => TemperatureUnit.Celsius,
                "f" or "fahrenheit" => TemperatureUnit.Fahrenheit,
                "k" or "kelvin" => TemperatureUnit.Kelvin,
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };
        }

        /// <summary>
        /// Converts a <see cref="TemperatureUnit"/> enum value to its string representation.
        /// </summary>
        /// <param name="unit">The <see cref="TemperatureUnit"/> enum value.</param>
        /// <returns>The string representation of the temperature unit.</returns>
        public static string ToUnitString(this TemperatureUnit unit)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => "°C",
                TemperatureUnit.Fahrenheit => "°F",
                TemperatureUnit.Kelvin => "K",
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };
        }

        /// <summary>
        /// Determines whether the specified string is a valid temperature unit.
        /// </summary>
        /// <param name="unit">The string to check.</param>
        /// <returns><c>true</c> if the string is a valid temperature unit; otherwise, <c>false</c>.</returns>
        public static bool IsValidUnitString(string unit)
        {
            return ValidUnits.Contains(unit.Replace("°", "").ToLower().Trim());
        }
    }

    /// <summary>
    /// Represents a temperature value in different units (Celsius, Fahrenheit, Kelvin).
    /// </summary>
    [JsonConverter(typeof(TemperatureConverter))]
    public struct Temperature : IComparable, IComparable<Temperature>, IEquatable<Temperature>, IFormattable
    {
        /// <summary>
        /// The temperature value in Kelvin.
        /// </summary>
        private double _kelvinValue;

        public static readonly Temperature MinValue = new Temperature(0, TemperatureUnit.Kelvin);
        public static readonly Temperature MaxValue = new Temperature(double.MaxValue, TemperatureUnit.Kelvin);

        public static Temperature FromCelsius(double value) => new Temperature(value, TemperatureUnit.Celsius);
        public static Temperature FromFahrenheit(double value) => new Temperature(value, TemperatureUnit.Fahrenheit);
        public static Temperature FromKelvin(double value) => new Temperature(value, TemperatureUnit.Kelvin);

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct with a value of 0 Kelvin.
        /// </summary>
        public Temperature()
        {
            _kelvinValue = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct with the specified value and unit.
        /// </summary>
        /// <param name="value">The temperature value.</param>
        /// <param name="unit">The unit of the temperature value.</param>
        /// <exception cref="ArgumentException">Thrown when the specified unit is unknown.</exception>
        public Temperature(double value, TemperatureUnit unit)
        {
            _kelvinValue = Math.Clamp(unit switch
            {
                TemperatureUnit.Celsius => value + 273.15,
                TemperatureUnit.Fahrenheit => (value + 459.67) * 5 / 9,
                TemperatureUnit.Kelvin => value,
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            }, 0, double.MaxValue);
        }

        private static double ConvertToKelvin(double value, TemperatureUnit unit)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value + 273.15,
                TemperatureUnit.Fahrenheit => (value + 459.67) * 5 / 9,
                TemperatureUnit.Kelvin => value,
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };
        }

        /// <summary>
        /// Gets or sets the temperature value in Celsius.
        /// </summary>
        /// <value>The temperature value in Celsius.</value>
        public double Celsius
        {
            get => Kelvin - 273.15;
            set => Kelvin = value + 273.15;
        }

        /// <summary>
        /// Gets or sets the temperature value in Fahrenheit.
        /// </summary>
        /// <value>The temperature value in Fahrenheit.</value>
        public double Fahrenheit
        {
            get => Kelvin * 9 / 5 - 459.67;
            set => Kelvin = (value + 459.67) * 5 / 9;
        }

        /// <summary>
        /// Gets or sets the temperature value in Kelvin.
        /// </summary>
        /// <value>The temperature value in Kelvin.</value>
        public double Kelvin
        {
            get => _kelvinValue;
            set 
            {
                _kelvinValue = Math.Clamp(value, 0, double.MaxValue);
            }
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is Temperature other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Object must be of type {nameof(Temperature)}");
        }

        /// <inheritdoc/>
        public int CompareTo(Temperature other)
        {
            return _kelvinValue.CompareTo(other._kelvinValue);
        }

        /// <inheritdoc/>
        public bool Equals(Temperature other)
        {
            return _kelvinValue.Equals(other._kelvinValue);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Temperature other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _kelvinValue.GetHashCode();
        }

        public static bool operator ==(Temperature left, Temperature right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Temperature left, Temperature right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Temperature left, Temperature right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Temperature left, Temperature right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Temperature left, Temperature right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Temperature left, Temperature right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            format = string.IsNullOrEmpty(format) ? "C" : format;
            IFormatProvider provider = formatProvider ?? CultureInfo.InvariantCulture;

            if (!TemperatureUnitExtensions.IsValidUnitString(format))
            {
                throw new FormatException($"Unknown temperature unit: {format}");
            }

            TemperatureUnit unit = TemperatureUnitExtensions.FromString(format);
            return unit switch
            {
                TemperatureUnit.Celsius => $"{Celsius.ToString(provider)} {unit.ToUnitString()}",
                TemperatureUnit.Fahrenheit => $"{Fahrenheit.ToString(provider)} {unit.ToUnitString()}",
                TemperatureUnit.Kelvin => $"{Kelvin.ToString(provider)} {unit.ToUnitString()}",
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString("C", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Converts a <see cref="Temperature"/> object to and from JSON using custom logic.
    /// Saves the temperature value in Kelvin.
    /// </summary>
    public class TemperatureConverter : JsonConverter<Temperature>
    {
        /// <inheritdoc/>
        public override Temperature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            double value = 0;
            TemperatureUnit unit = TemperatureUnit.Kelvin;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Temperature(value, unit);
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
                    unit = TemperatureUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
                }
                else
                {
                    throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Temperature value, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            writer.WriteStartObject();
            writer.WriteNumber(valuePropertyName, value.Kelvin);
            writer.WriteString(unitPropertyName, TemperatureUnit.Kelvin.ToUnitString());
            writer.WriteEndObject();
        }
    }
}
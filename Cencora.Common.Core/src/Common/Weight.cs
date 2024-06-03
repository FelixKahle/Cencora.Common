// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents the unit of weight.
    /// </summary>
    public enum WeightUnit
    {
        Micrograms,
        Milligrams,
        Grams,
        Kilograms,
        Tons,
        Pounds,
        Ounces,
        Stones,
        Carats,
        LongTons,
        ShortTons
    }

    /// <summary>
    /// Provides extension methods for the <see cref="WeightUnit"/> enum.
    /// </summary>
    public static class WeightUnitExtensions
    {
        /// <summary>
        /// Converts the specified weight unit to a string representation.
        /// </summary>
        /// <param name="unit">The weight unit to convert.</param>
        /// <returns>The string representation of the weight unit.</returns>
        /// <exception cref="ArgumentException">Thrown if the weight unit is invalid.</exception>
        public static string ToUnitString(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Micrograms => "µg",
                WeightUnit.Milligrams => "mg",
                WeightUnit.Grams => "g",
                WeightUnit.Kilograms => "kg",
                WeightUnit.Tons => "t",
                WeightUnit.Pounds => "lb",
                WeightUnit.Ounces => "oz",
                WeightUnit.Stones => "st",
                WeightUnit.Carats => "ct",
                WeightUnit.LongTons => "lt",
                WeightUnit.ShortTons => "st",
                _ => throw new ArgumentException("Invalid weight unit", nameof(unit)),
            };
        }

        /// <summary>
        /// Converts the specified string to a weight unit.
        /// </summary>
        /// <param name="unit">The string to convert.</param>
        /// <returns>The weight unit represented by the string.</returns>
        /// <exception cref="ArgumentException">Thrown if the string is not a valid weight unit.</exception>
        public static WeightUnit FromString(string unit)
        {
            return unit.ToLower().Trim() switch
            {
                "µg" or "micrograms" or "microgram" => WeightUnit.Micrograms,
                "mg" or "milligrams" or "milligram" => WeightUnit.Milligrams,
                "g" or "grams" or "gram" => WeightUnit.Grams,
                "kg" or "kilograms" or "kilogram" => WeightUnit.Kilograms,
                "t" or "tons" or "ton" => WeightUnit.Tons,
                "lb" or "pounds" or "pound" => WeightUnit.Pounds,
                "oz" or "ounces" or "ounce" => WeightUnit.Ounces,
                "st" or "stones" or "stone" => WeightUnit.Stones,
                "ct" or "carats" or "carat" => WeightUnit.Carats,
                "lt" or "longtons" or "longton" => WeightUnit.LongTons,
                "st" or "shorttons" or "shortton" => WeightUnit.ShortTons,
                _ => throw new ArgumentException("Invalid weight unit", nameof(unit)),
            };
        }
    }

    /// <summary>
    /// Represents a weight measurement.
    /// An error is thrown if a negative value is assigned to the weight.
    /// </summary>
    [JsonConverter(typeof(WeightConverter))]
    public struct Weight : IComparable, IComparable<Weight>, IEquatable<Weight>, IFormattable
    {
        /// <summary>
        /// The internal weight in grams.
        /// </summary>
        private double _grams; 

        /// <summary>
        /// Initializes a new instance of the <see cref="Weight"/> struct with a default value of 0 grams.
        /// </summary>
        public Weight()
        {
            _grams = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Weight"/> struct with the specified value and unit.
        /// </summary>
        /// <param name="value">The value of the weight.</param>
        /// <param name="unit">The unit of the weight.</param>
        public Weight(double value, WeightUnit unit)
        {
            if (value < 0)
            {
                throw new ArgumentException("Weight value cannot be negative", nameof(value));
            }

            _grams = ConvertToGrams(value, unit);
        }

        /// <summary>
        /// Converts the specified value from the given weight unit to grams.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="unit">The weight unit of the value.</param>
        /// <returns>The converted value in grams.</returns>
        private static double ConvertToGrams(double value, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Micrograms => value / 1_000_000,
                WeightUnit.Milligrams => value / 1000,
                WeightUnit.Grams => value,
                WeightUnit.Kilograms => value * 1000,
                WeightUnit.Tons => value * 1_000_000,
                WeightUnit.Pounds => value * 453.59237,
                WeightUnit.Ounces => value * 28.349523125,
                WeightUnit.Stones => value * 6350.29318,
                WeightUnit.Carats => value / 5,
                WeightUnit.LongTons => value * 1_016_046.9088,
                WeightUnit.ShortTons => value * 907_184.74,
                _ => throw new ArgumentException("Invalid weight unit", nameof(unit)),
            };
        }

        /// <summary>
        /// Converts a weight value from grams to the specified unit.
        /// </summary>
        /// <param name="grams">The weight value in grams.</param>
        /// <param name="unit">The unit to convert the weight to.</param>
        /// <returns>The converted weight value.</returns>
        private static double ConvertFromGrams(double grams, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Micrograms => grams * 1_000_000,
                WeightUnit.Milligrams => grams * 1000,
                WeightUnit.Grams => grams,
                WeightUnit.Kilograms => grams / 1000,
                WeightUnit.Tons => grams / 1_000_000,
                WeightUnit.Pounds => grams / 453.59237,
                WeightUnit.Ounces => grams / 28.349523125,
                WeightUnit.Stones => grams / 6350.29318,
                WeightUnit.Carats => grams * 5,
                WeightUnit.LongTons => grams / 1_016_046.9088,
                WeightUnit.ShortTons => grams / 907_184.74,
                _ => throw new ArgumentException("Invalid weight unit", nameof(unit)),
            };
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is Weight weight)
            {
                return _grams.CompareTo(weight._grams);
            }

            throw new ArgumentException("Object is not a Weight", nameof(obj));
        }

        /// <inheritdoc/>
        public int CompareTo(Weight other)
        {
            return _grams.CompareTo(other._grams);
        }

        /// <inheritdoc/>
        public bool Equals(Weight other)
        {
            return _grams.Equals(other._grams);
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            format = string.IsNullOrEmpty(format) ? "g" : format;
            formatProvider = formatProvider ?? CultureInfo.InvariantCulture;

            WeightUnit unit = WeightUnitExtensions.FromString(format);
            double value = ConvertFromGrams(_grams, unit);
            
            return $"{value.ToString(formatProvider)} {unit.ToUnitString()}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Weight weight && Equals(weight);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _grams.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString("g", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the weight in micrograms.
        /// </summary>
        public double Micrograms
        {
            get => ConvertFromGrams(_grams, WeightUnit.Micrograms);
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Micrograms);
            }
        }

        /// <summary>
        /// Gets or sets the weight in milligrams.
        /// </summary>
        public double Milligrams
        {
            get => ConvertFromGrams(_grams, WeightUnit.Milligrams);
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Milligrams);
            }
        }

        /// <summary>
        /// Gets or sets the weight in grams.
        /// </summary>
        public double Grams
        {
            get => _grams;
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }
                 _grams = ConvertToGrams(value, WeightUnit.Grams);
            }
        }

        /// <summary>
        /// Gets or sets the weight in kilograms.
        /// </summary>
        public double Kilograms
        {
            get => ConvertFromGrams(_grams, WeightUnit.Kilograms);
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Kilograms);
            
            }
        }

        /// <summary>
        /// Gets or sets the weight in tons.
        /// </summary>
        public double Tons
        {
            get => ConvertFromGrams(_grams, WeightUnit.Tons);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Tons);
            }
        }

        /// <summary>
        /// Gets or sets the weight in pounds.
        /// </summary>
        public double Pounds
        {
            get => ConvertFromGrams(_grams, WeightUnit.Pounds);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Pounds);
            }
        }

        /// <summary>
        /// Gets or sets the weight in ounces.
        /// </summary>
        public double Ounces
        {
            get => ConvertFromGrams(_grams, WeightUnit.Ounces);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Ounces);
            }
        }

        /// <summary>
        /// Gets or sets the weight in stones.
        /// </summary>
        public double Stones
        {
            get => ConvertFromGrams(_grams, WeightUnit.Stones);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Stones);
            }
        }

        /// <summary>
        /// Gets or sets the weight in carats.
        /// </summary>
        public double Carats
        {
            get => ConvertFromGrams(_grams, WeightUnit.Carats);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.Carats);
            }
        }

        /// <summary>
        /// Gets or sets the weight in long tons.
        /// </summary>
        public double LongTons
        {
            get => ConvertFromGrams(_grams, WeightUnit.LongTons);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.LongTons);
            }
        }

        /// <summary>
        /// Gets or sets the weight in short tons.
        /// </summary>
        public double ShortTons
        {
            get => ConvertFromGrams(_grams, WeightUnit.ShortTons);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Weight value cannot be negative", nameof(value));
                }

                _grams = ConvertToGrams(value, WeightUnit.ShortTons);
            }
        }

        public static bool operator ==(Weight left, Weight right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Weight left, Weight right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Provides a custom JSON converter for the <see cref="Weight"/> struct.
    /// Saves the weight in grams.
    /// </summary>
    public class WeightConverter : JsonConverter<Weight>
    {
        /// <inheritdoc/>
        public override Weight Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            double value = 0;
            WeightUnit unit = WeightUnit.Grams;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Weight(value, unit);
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
                    unit = WeightUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
                }
                else
                {
                    throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Weight value, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
            string unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

            writer.WriteStartObject();
            writer.WriteNumber(valuePropertyName, value.Grams);
            writer.WriteString(unitPropertyName, WeightUnit.Grams.ToUnitString());
            writer.WriteEndObject();
        }
    }
}
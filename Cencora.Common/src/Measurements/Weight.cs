// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

/// <summary>
/// Represents the unit of weight.
/// </summary>
public enum WeightUnit
{
    Microgram,
    Milligram,
    Gram,
    Kilogram,
    Ton,
    Pound,
    Ounce,
    Stone,
    Carat,
    LongTon,
    ShortTon
}

/// <summary>
/// Provides extension methods for the <see cref="WeightUnit"/> enum.
/// </summary>
public static class WeightUnitExtensions
{
    /// <summary>
    /// The set of valid weight units.
    /// </summary>
    private static readonly HashSet<string> ValidUnits =
    [
        "µg", "micrograms", "microgram",
        "mg", "milligrams", "milligram",
        "g", "grams", "gram",
        "kg", "kilograms", "kilogram",
        "t", "tons", "ton",
        "lb", "pounds", "pound",
        "oz", "ounces", "ounce",
        "st.", "stones", "stone",
        "ct", "carats", "carat",
        "lt", "longtons", "longton",
        "st", "shorttons", "shortton"
    ];

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
            WeightUnit.Microgram => "µg",
            WeightUnit.Milligram => "mg",
            WeightUnit.Gram => "g",
            WeightUnit.Kilogram => "kg",
            WeightUnit.Ton => "t",
            WeightUnit.Pound => "lb",
            WeightUnit.Ounce => "oz",
            WeightUnit.Stone => "st.",
            WeightUnit.Carat => "ct",
            WeightUnit.LongTon => "lt",
            WeightUnit.ShortTon => "st",
            _ => throw new ArgumentException("Invalid weight unit", nameof(unit))
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
        return unit.ToLower().Replace(" ", "").Trim() switch
        {
            "µg" or "micrograms" or "microgram" => WeightUnit.Microgram,
            "mg" or "milligrams" or "milligram" => WeightUnit.Milligram,
            "g" or "grams" or "gram" => WeightUnit.Gram,
            "kg" or "kilograms" or "kilogram" => WeightUnit.Kilogram,
            "t" or "tons" or "ton" => WeightUnit.Ton,
            "lb" or "pounds" or "pound" => WeightUnit.Pound,
            "oz" or "ounces" or "ounce" => WeightUnit.Ounce,
            "st." or "stones" or "stone" => WeightUnit.Stone,
            "ct" or "carats" or "carat" => WeightUnit.Carat,
            "lt" or "longtons" or "longton" => WeightUnit.LongTon,
            "st" or "shorttons" or "shortton" => WeightUnit.ShortTon,
            _ => throw new ArgumentException("Invalid weight unit", nameof(unit))
        };
    }

    /// <summary>
    /// Determines whether the specified string is a valid weight unit.
    /// </summary>
    /// <param name="unit">The string to check.</param>
    /// <returns>True if the string is a valid weight unit, otherwise false.</returns>
    public static bool IsValidUnitString(string unit)
    {
        return ValidUnits.Contains(unit.ToLower().Replace(" ", "").Trim());
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

    public static readonly Weight Zero = new(0, WeightUnit.Gram);
    public static readonly Weight MinValue = new(0, WeightUnit.Gram);
    public static readonly Weight MaxValue = new(double.MaxValue, WeightUnit.Gram);
    public static readonly Weight Infinity = new(double.PositiveInfinity, WeightUnit.Gram);

    public static Weight FromMicrograms(double value) => new(value, WeightUnit.Microgram);
    public static Weight FromMilligrams(double value) => new(value, WeightUnit.Milligram);
    public static Weight FromGrams(double value) => new(value, WeightUnit.Gram);
    public static Weight FromKilograms(double value) => new(value, WeightUnit.Kilogram);
    public static Weight FromTons(double value) => new(value, WeightUnit.Ton);
    public static Weight FromPounds(double value) => new(value, WeightUnit.Pound);
    public static Weight FromOunces(double value) => new(value, WeightUnit.Ounce);
    public static Weight FromStones(double value) => new(value, WeightUnit.Stone);
    public static Weight FromCarats(double value) => new(value, WeightUnit.Carat);
    public static Weight FromLongTons(double value) => new(value, WeightUnit.LongTon);
    public static Weight FromShortTons(double value) => new(value, WeightUnit.ShortTon);

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
        _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), unit);
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
            WeightUnit.Microgram => value / 1_000_000,
            WeightUnit.Milligram => value / 1000,
            WeightUnit.Gram => value,
            WeightUnit.Kilogram => value * 1000,
            WeightUnit.Ton => value * 1_000_000,
            WeightUnit.Pound => value * 453.59237,
            WeightUnit.Ounce => value * 28.349523125,
            WeightUnit.Stone => value * 6350.29318,
            WeightUnit.Carat => value / 5,
            WeightUnit.LongTon => value * 1_016_046.9088,
            WeightUnit.ShortTon => value * 907_184.74,
            _ => throw new ArgumentException("Invalid weight unit", nameof(unit))
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
            WeightUnit.Microgram => grams * 1_000_000,
            WeightUnit.Milligram => grams * 1000,
            WeightUnit.Gram => grams,
            WeightUnit.Kilogram => grams / 1000,
            WeightUnit.Ton => grams / 1_000_000,
            WeightUnit.Pound => grams / 453.59237,
            WeightUnit.Ounce => grams / 28.349523125,
            WeightUnit.Stone => grams / 6350.29318,
            WeightUnit.Carat => grams * 5,
            WeightUnit.LongTon => grams / 1_016_046.9088,
            WeightUnit.ShortTon => grams / 907_184.74,
            _ => throw new ArgumentException("Invalid weight unit", nameof(unit))
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
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        format = string.IsNullOrEmpty(format) ? "g" : format;
        formatProvider ??= CultureInfo.InvariantCulture;

        if (!WeightUnitExtensions.IsValidUnitString(format))
        {
            throw new FormatException("Invalid format string");
        }

        var unit = WeightUnitExtensions.FromString(format);
        var value = ConvertFromGrams(_grams, unit);
            
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
        get => ConvertFromGrams(_grams, WeightUnit.Microgram);
        init => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Microgram);
    }

    /// <summary>
    /// Gets or sets the weight in milligrams.
    /// </summary>
    public double Milligrams
    {
        get => ConvertFromGrams(_grams, WeightUnit.Milligram);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Milligram);
    }

    /// <summary>
    /// Gets or sets the weight in grams.
    /// </summary>
    public double Grams
    {
        get => _grams;
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Gram);
    }

    /// <summary>
    /// Gets or sets the weight in kilograms.
    /// </summary>
    public double Kilograms
    {
        get => ConvertFromGrams(_grams, WeightUnit.Kilogram);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Kilogram);
    }

    /// <summary>
    /// Gets or sets the weight in tons.
    /// </summary>
    public double Tons
    {
        get => ConvertFromGrams(_grams, WeightUnit.Ton);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Ton);
    }

    /// <summary>
    /// Gets or sets the weight in pounds.
    /// </summary>
    public double Pounds
    {
        get => ConvertFromGrams(_grams, WeightUnit.Pound);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Pound);
    }

    /// <summary>
    /// Gets or sets the weight in ounces.
    /// </summary>
    public double Ounces
    {
        get => ConvertFromGrams(_grams, WeightUnit.Ounce);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Ounce);
    }

    /// <summary>
    /// Gets or sets the weight in stones.
    /// </summary>
    public double Stones
    {
        get => ConvertFromGrams(_grams, WeightUnit.Stone);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Stone);
    }

    /// <summary>
    /// Gets or sets the weight in carats.
    /// </summary>
    public double Carats
    {
        get => ConvertFromGrams(_grams, WeightUnit.Carat);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.Carat);
    }

    /// <summary>
    /// Gets or sets the weight in long tons.
    /// </summary>
    public double LongTons
    {
        get => ConvertFromGrams(_grams, WeightUnit.LongTon);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.LongTon);
    }

    /// <summary>
    /// Gets or sets the weight in short tons.
    /// </summary>
    public double ShortTons
    {
        get => ConvertFromGrams(_grams, WeightUnit.ShortTon);
        set => _grams = ConvertToGrams(Math.Clamp(value, 0, double.MaxValue), WeightUnit.ShortTon);
    }

    public static bool operator ==(Weight left, Weight right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Weight left, Weight right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(Weight left, Weight right)
    {
        return left.Grams < right.Grams;
    }

    public static bool operator <=(Weight left, Weight right)
    {
        return left.Grams <= right.Grams;
    }

    public static bool operator >(Weight left, Weight right)
    {
        return left.Grams > right.Grams;
    }

    public static bool operator >=(Weight left, Weight right)
    {
        return left.Grams >= right.Grams;
    }

    public static Weight operator +(Weight left, Weight right)
    {
        var value = Math.Clamp(left.Grams + right.Grams, 0, double.MaxValue);
        return new Weight(value, WeightUnit.Gram);
    }

    public static Weight operator -(Weight left, Weight right)
    {
        var value = Math.Clamp(left.Grams - right.Grams, 0, double.MaxValue);
        return new Weight(value, WeightUnit.Gram);
    }
}
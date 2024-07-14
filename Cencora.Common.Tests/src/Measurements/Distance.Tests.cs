// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Cencora.Common.Measurements;
using Xunit.Abstractions;

namespace Cencora.Common.Tests.Measurements;

public class DistanceUnitExtensionsTests
{
    [Theory]
    [InlineData(DistanceUnit.Millimeter, "mm")]
    [InlineData(DistanceUnit.Centimeter, "cm")]
    [InlineData(DistanceUnit.Meter, "m")]
    [InlineData(DistanceUnit.Kilometer, "km")]
    [InlineData(DistanceUnit.Inch, "in")]
    [InlineData(DistanceUnit.Foot, "ft")]
    [InlineData(DistanceUnit.Yard, "yd")]
    [InlineData(DistanceUnit.Mile, "mi")]
    [InlineData(DistanceUnit.NauticalMile, "nmi")]
    public void ToUnitString_WithDistanceUnit_ShouldReturnString(DistanceUnit unit, string expected)
    {
        var result = unit.ToUnitString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToUnitString_WithInvalidDistanceUnit_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => ((DistanceUnit)int.MaxValue).ToUnitString());
    }

    [Theory]
    [InlineData("mm", DistanceUnit.Millimeter)]
    [InlineData("millimeters", DistanceUnit.Millimeter)]
    [InlineData("millimeter", DistanceUnit.Millimeter)]
    [InlineData("Millimeter", DistanceUnit.Millimeter)]
    [InlineData("cm", DistanceUnit.Centimeter)]
    [InlineData("centimeters", DistanceUnit.Centimeter)]
    [InlineData("centimeter", DistanceUnit.Centimeter)]
    [InlineData("Centimeter", DistanceUnit.Centimeter)]
    [InlineData("m", DistanceUnit.Meter)]
    [InlineData("meters", DistanceUnit.Meter)]
    [InlineData("meter", DistanceUnit.Meter)]
    [InlineData("Meter", DistanceUnit.Meter)]
    [InlineData("km", DistanceUnit.Kilometer)]
    [InlineData("kilometers", DistanceUnit.Kilometer)]
    [InlineData("kilometer", DistanceUnit.Kilometer)]
    [InlineData("Kilometer", DistanceUnit.Kilometer)]
    [InlineData("in", DistanceUnit.Inch)]
    [InlineData("inches", DistanceUnit.Inch)]
    [InlineData("inch", DistanceUnit.Inch)]
    [InlineData("Inch", DistanceUnit.Inch)]
    [InlineData("ft", DistanceUnit.Foot)]
    [InlineData("feet", DistanceUnit.Foot)]
    [InlineData("foot", DistanceUnit.Foot)]
    [InlineData("Foot", DistanceUnit.Foot)]
    [InlineData("yd", DistanceUnit.Yard)]
    [InlineData("yards", DistanceUnit.Yard)]
    [InlineData("yard", DistanceUnit.Yard)]
    [InlineData("Yard", DistanceUnit.Yard)]
    [InlineData("mi", DistanceUnit.Mile)]
    [InlineData("miles", DistanceUnit.Mile)]
    [InlineData("mile", DistanceUnit.Mile)]
    [InlineData("Mile", DistanceUnit.Mile)]
    [InlineData("nmi", DistanceUnit.NauticalMile)]
    [InlineData("nautical miles", DistanceUnit.NauticalMile)]
    [InlineData("nautical mile", DistanceUnit.NauticalMile)]
    [InlineData("Nautical Mile", DistanceUnit.NauticalMile)]
    public void FromString_WithValidString_ShouldReturnDistanceUnit(string input, DistanceUnit expected)
    {
        var distanceUnit = DistanceUnitExtensions.FromString(input);
        Assert.Equal(expected, distanceUnit);
    }

    [Fact]
    public void FromString_WithInvalidString_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => DistanceUnitExtensions.FromString("invalid"));
    }

    [Theory]
    [InlineData("mm")]
    [InlineData("millimeters")]
    [InlineData("millimeter")]
    [InlineData("Millimeter")]
    [InlineData("cm")]
    [InlineData("centimeters")]
    [InlineData("centimeter")]
    [InlineData("Centimeter")]
    [InlineData("m")]
    [InlineData("meters")]
    [InlineData("meter")]
    [InlineData("Meter")]
    [InlineData("km")]
    [InlineData("kilometers")]
    [InlineData("kilometer")]
    [InlineData("Kilometer")]
    [InlineData("in")]
    [InlineData("inches")]
    [InlineData("inch")]
    [InlineData("Inch")]
    [InlineData("ft")]
    [InlineData("feet")]
    [InlineData("foot")]
    [InlineData("Foot")]
    [InlineData("yd")]
    [InlineData("yards")]
    [InlineData("yard")]
    [InlineData("Yard")]
    [InlineData("mi")]
    [InlineData("miles")]
    [InlineData("mile")]
    [InlineData("Mile")]
    [InlineData("nmi")]
    [InlineData("nautical miles")]
    [InlineData("nautical mile")]
    [InlineData("Nautical Mile")]
    public void IsValidUnitString_WithValidFormat_ShouldReturnTrue(string input)
    {
        Assert.True(DistanceUnitExtensions.IsValidUnitString(input));
    }

    [Fact]
    public void IsValidUnitString_WithInvalidFormat_ShouldReturnFalse()
    {
        Assert.False(DistanceUnitExtensions.IsValidUnitString("invalid"));
    }
}

public class DistanceTests(ITestOutputHelper output)
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        var distance = new Distance();
        Assert.Equal(0, distance.Meters);
    }

    [Theory]
    [InlineData(1, DistanceUnit.Millimeter, 0.001)]
    [InlineData(1, DistanceUnit.Centimeter, 0.01)]
    [InlineData(1, DistanceUnit.Meter, 1)]
    [InlineData(1, DistanceUnit.Kilometer, 1000)]
    [InlineData(1, DistanceUnit.Inch, 0.0254)]
    [InlineData(1, DistanceUnit.Foot, 0.3048)]
    [InlineData(1, DistanceUnit.Yard, 0.9144)]
    [InlineData(1, DistanceUnit.Mile, 1609.34)]
    [InlineData(1, DistanceUnit.NauticalMile, 1852)]
    public void Constructor_WithValidParameters_ShouldCreateInstance(double value, DistanceUnit unit, double expectedMeters)
    {
        var distance = new Distance(value, unit);
        Assert.Equal(expectedMeters, distance.Meters, 0.0001);
    }

    [Fact]
    public void Constructor_WithInvalidParameters_ShouldSetToZero()
    {
        var distance = new Distance(-1, DistanceUnit.Meter);
        Assert.Equal(0, distance.Meters);

        distance = new Distance(-1, DistanceUnit.Millimeter);
        Assert.Equal(0, distance.Millimeters);
    }

    [Fact]
    public void Equals_WithEqualInstances_ShouldReturnTrue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1.Equals(distance2));
        Assert.True(distance1 == distance2);
        Assert.False(distance1 != distance2);
    }

    [Fact]
    public void Equals_WithDifferentInstances_ShouldReturnFalse()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.False(distance1.Equals(distance2));
        Assert.False(distance1 == distance2);
        Assert.True(distance1 != distance2);
    }

    [Fact]
    public void Greater_WithGreaterInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1 > distance2);
    }

    [Fact]
    public void Greater_WithEqualInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.False(distance1 > distance2);
    }

    [Fact]
    public void Greater_WithSmallerInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.False(distance1 > distance2);
    }

    [Fact]
    public void GreaterOrEqual_WithGreaterInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1 >= distance2);
    }

    [Fact]
    public void GreaterOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1 >= distance2);
    }

    [Fact]
    public void GreaterOrEqual_WithSmallerInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.False(distance1 >= distance2);
    }

    [Fact]
    public void Less_WithGreaterInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.False(distance1 < distance2);
    }

    [Fact]
    public void Less_WithEqualInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.False(distance1 < distance2);
    }

    [Fact]
    public void Less_WithSmallerInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.True(distance1 < distance2);
    }

    [Fact]
    public void LessOrEqual_WithGreaterInstance_ShouldReturnFalse()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.False(distance1 <= distance2);
    }

    [Fact]
    public void LessOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1 <= distance2);
    }

    [Fact]
    public void LessOrEqual_WithSmallerInstance_ShouldReturnTrue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.True(distance1 <= distance2);
    }

    [Fact]
    public void Add_WithSameUnit_ShouldReturnCorrectValue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        var result = distance1 + distance2;
        Assert.Equal(3, result.Meters, 0.0001);
    }

    [Fact]
    public void Add_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(4, DistanceUnit.Foot);
        var result = distance1 + distance2;
        Assert.Equal(2.2192, result.Meters, 0.0001);
    }

    [Fact]
    public void Subtract_WithSameUnit_ShouldReturnCorrectValue()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        var result = distance1 - distance2;
        Assert.Equal(1, result.Meters, 0.0001);
    }

    [Fact]
    public void Subtract_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var distance1 = new Distance(1, DistanceUnit.Kilometer);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        var result = distance1 - distance2;
        Assert.Equal(998, result.Meters, 0.0001);
    }

    [Fact]
    public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(distance1.GetHashCode(), distance2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.NotEqual(distance1.GetHashCode(), distance2.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithEqualInstances_ShouldReturnZero()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(0, distance1.CompareTo(distance2));
    }

    [Fact]
    public void CompareTo_WithGreaterInstance_ShouldReturnPositive()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        var distance2 = new Distance(2, DistanceUnit.Meter);
        Assert.True(distance1.CompareTo(distance2) < 0);
    }

    [Fact]
    public void CompareTo_WithSmallerInstance_ShouldReturnNegative()
    {
        var distance1 = new Distance(2, DistanceUnit.Meter);
        var distance2 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1.CompareTo(distance2) > 0);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        var distance1 = new Distance(1, DistanceUnit.Meter);
        Assert.True(distance1.CompareTo(null) > 0);
    }

    [Fact]
    public void Millimeters_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(1000, distance.Millimeters, 0.0001);
    }

    [Fact]
    public void Centimeters_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(100, distance.Centimeters, 0.0001);
    }

    [Fact]
    public void Kilometers_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(0.001, distance.Kilometers, 0.0001);
    }

    [Fact]
    public void Inches_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(39.3701, distance.Inches, 0.0001);
    }

    [Fact]
    public void Feet_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(3.28084, distance.Feet, 0.0001);
    }

    [Fact]
    public void Yards_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(1.09361, distance.Yards, 0.0001);
    }

    [Fact]
    public void Miles_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(0.000621371, distance.Miles, 0.0001);
    }

    [Fact]
    public void NauticalMiles_WithOneMeter_ShouldReturnCorrectValue()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(0.000539957, distance.NauticalMiles, 0.0001);
    }

    [Theory]
    [InlineData("cm", "cm")]
    [InlineData("centimeters", "cm")]
    [InlineData("centimeter", "cm")]
    [InlineData("Centimeter", "cm")]
    [InlineData("m", "m")]
    [InlineData("meters", "m")]
    [InlineData("meter", "m")]
    [InlineData("Meter", "m")]
    [InlineData("km", "km")]
    [InlineData("kilometers", "km")]
    [InlineData("kilometer", "km")]
    [InlineData("Kilometer", "km")]
    [InlineData("in", "in")]
    [InlineData("inches", "in")]
    [InlineData("inch", "in")]
    [InlineData("Inch", "in")]
    [InlineData("ft", "ft")]
    [InlineData("feet", "ft")]
    [InlineData("foot", "ft")]
    [InlineData("Foot", "ft")]
    [InlineData("yd", "yd")]
    [InlineData("yards", "yd")]
    [InlineData("yard", "yd")]
    [InlineData("Yard", "yd")]
    [InlineData("mi", "mi")]
    [InlineData("miles", "mi")]
    [InlineData("mile", "mi")]
    [InlineData("Mile", "mi")]
    [InlineData("nmi", "nmi")]
    [InlineData("nautical miles", "nmi")]
    [InlineData("nautical mile", "nmi")]
    [InlineData("Nautical Mile", "nmi")]
    public void ToString_WithValidFormat_ReturnsCorrectUnit(string fmt, string expectedUnit)
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        var result = distance.ToString(fmt);
        Assert.EndsWith(expectedUnit, result);
    }

    [Fact]
    public void ToString_WithInvalidFormat_ShouldThrowException()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        Assert.Throws<FormatException>(() => distance.ToString("invalid"));
    }

    [Fact]
    public void SetDistance_NegativeValue_ShouldSetToZero()
    {
        var distance = new Distance
        {
            Meters = -1
        };

        Assert.Equal(0, distance.Meters);

        distance.Millimeters = -1;
        Assert.Equal(0, distance.Millimeters);

        distance.Centimeters = -1;
        Assert.Equal(0, distance.Centimeters);

        distance.Kilometers = -1;
        Assert.Equal(0, distance.Kilometers);

        distance.Inches = -1;
        Assert.Equal(0, distance.Inches);

        distance.Feet = -1;
        Assert.Equal(0, distance.Feet);

        distance.Yards = -1;
        Assert.Equal(0, distance.Yards);

        distance.Miles = -1;
        Assert.Equal(0, distance.Miles);

        distance.NauticalMiles = -1;
        Assert.Equal(0, distance.NauticalMiles);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="DistanceConverter"/> to be used when serializing and deserializing <see cref="Distance"/> objects.
    /// </summary>
    [Fact]
    public void Json_Serialize_ShouldReturnCorrectJson()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        var json = JsonSerializer.Serialize(distance, _options);
        output.WriteLine($"{nameof(Json_Serialize_ShouldReturnCorrectJson)}: serialized JSON: {json}");

        Assert.Equal("{\"value\":1,\"unit\":\"m\"}", json, true, true, true, true);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="DistanceConverter"/> to be used when serializing and deserializing <see cref="Distance"/> objects.
    /// </summary>
    [Fact]
    public void Json_Deserialize_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1,\"unit\":\"m\"}";
        var distance = JsonSerializer.Deserialize<Distance>(json, _options);

        var expected = new Distance(1, DistanceUnit.Meter);
        Assert.Equal(expected, distance);
    }
}

public class DistanceConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Write_WithDistance_ShouldReturnCorrectJson()
    {
        var distance = new Distance(1, DistanceUnit.Meter);
        var json = JsonSerializer.Serialize(distance, _options);

        Assert.Equal("{\"value\":1,\"unit\":\"m\"}", json, true, true, true, true);
    }

    [Fact]
    public void Read_WithDistanceUnitMillimeters_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1,\"unit\":\"mm\"}";
        var distance = JsonSerializer.Deserialize<Distance>(json, _options);

        var expected = new Distance(1, DistanceUnit.Millimeter);
        Assert.Equal(expected, distance);
    }
}
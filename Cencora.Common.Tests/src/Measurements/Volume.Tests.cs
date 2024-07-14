// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Cencora.Common.Measurements;
using Xunit.Abstractions;

namespace Cencora.Common.Tests.Measurements;

public class VolumeUnitExtensionsTests
{
    [Theory]
    [InlineData(VolumeUnit.CubicCentimeter, "cm³")]
    [InlineData(VolumeUnit.CubicMeter, "m³")]
    [InlineData(VolumeUnit.CubicFeet, "ft³")]
    [InlineData(VolumeUnit.Liter, "l")]
    [InlineData(VolumeUnit.Milliliter, "ml")]
    [InlineData(VolumeUnit.Gallon, "gal")]
    public void ToUnitString_WithVolumeUnit_ShouldReturnString(VolumeUnit unit, string expected)
    {
        var result = unit.ToUnitString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToUnitString_WithInvalidVolumeUnit_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => ((VolumeUnit)int.MaxValue).ToUnitString());
    }

    [Theory]
    [InlineData("cm³", VolumeUnit.CubicCentimeter)]
    [InlineData("cm3", VolumeUnit.CubicCentimeter)]
    [InlineData("CM3", VolumeUnit.CubicCentimeter)]
    [InlineData("Cubic Centimeter", VolumeUnit.CubicCentimeter)]
    [InlineData("m³", VolumeUnit.CubicMeter)]
    [InlineData("m3", VolumeUnit.CubicMeter)]
    [InlineData("M3", VolumeUnit.CubicMeter)]
    [InlineData("Cubic Meter", VolumeUnit.CubicMeter)]
    [InlineData("ft³", VolumeUnit.CubicFeet)]
    [InlineData("ft3", VolumeUnit.CubicFeet)]
    [InlineData("FT3", VolumeUnit.CubicFeet)]
    [InlineData("Cubic Feet", VolumeUnit.CubicFeet)]
    [InlineData("l", VolumeUnit.Liter)]
    [InlineData("L", VolumeUnit.Liter)]
    [InlineData("Liter", VolumeUnit.Liter)]
    [InlineData("ml", VolumeUnit.Milliliter)]
    [InlineData("ML", VolumeUnit.Milliliter)]
    [InlineData("Milliliter", VolumeUnit.Milliliter)]
    [InlineData("gal", VolumeUnit.Gallon)]
    [InlineData("GAL", VolumeUnit.Gallon)]
    [InlineData("Gallon", VolumeUnit.Gallon)]
    public void FromString_WithValidString_ShouldReturnDistanceUnit(string input, VolumeUnit expected)
    {
        var result = VolumeUnitExtensions.FromString(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FromString_WithInvalidString_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => VolumeUnitExtensions.FromString("invalid"));
    }

    [Theory]
    [InlineData("cm³")]
    [InlineData("cm3")]
    [InlineData("CM3")]
    [InlineData("Cubic Centimeter")]
    [InlineData("m³")]
    [InlineData("m3")]
    [InlineData("M3")]
    [InlineData("Cubic Meter")]
    [InlineData("ft³")]
    [InlineData("ft3")]
    [InlineData("FT3")]
    [InlineData("Cubic Feet")]
    [InlineData("l")]
    [InlineData("L")]
    [InlineData("Liter")]
    [InlineData("ml")]
    [InlineData("ML")]
    [InlineData("Milliliter")]
    [InlineData("gal")]
    [InlineData("GAL")]
    [InlineData("Gallon")]
    public void IsValidUnitString_WithValidFormat_ShouldReturnTrue(string input)
    {
        Assert.True(VolumeUnitExtensions.IsValidUnitString(input));
    }

    [Fact]
    public void IsValidUnitString_WithInvalidFormat_ShouldReturnFalse()
    {
        Assert.False(VolumeUnitExtensions.IsValidUnitString("invalid"));
    }
}

public class VolumeTests(ITestOutputHelper output)
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        var volume = new Volume();
        Assert.Equal(0, volume.CubicMeters);
    }

    [Fact]
    public void Constructor_WithInvalidParameters_ShouldSetToZero()
    {
        var volume = new Volume(-1, VolumeUnit.CubicMeter);
        Assert.Equal(0, volume.CubicMeters);

        volume = new Volume(-1, VolumeUnit.CubicFeet);
        Assert.Equal(0, volume.CubicFeet);
    }

    [Theory]
    [InlineData(1, VolumeUnit.CubicCentimeter, 0.000001)]
    [InlineData(1, VolumeUnit.CubicMeter, 1)]
    [InlineData(1, VolumeUnit.CubicFeet, 0.028316846592)]
    [InlineData(1, VolumeUnit.Liter, 0.001)]
    [InlineData(1, VolumeUnit.Milliliter, 0.000001)]
    [InlineData(1, VolumeUnit.Gallon, 0.003785411784)]
    public void Constructor_WithParameters_ShouldCreateInstance(double value, VolumeUnit unit, double expectedCubicMeters)
    {
        var volume = new Volume(value, unit);
        Assert.Equal(expectedCubicMeters, volume.CubicMeters, 0.00001);
    }

    [Fact]
    public void Equals_WithEqualInstances_ShouldReturnTrue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1.Equals(volume2));
        Assert.True(volume1 == volume2);
        Assert.False(volume1 != volume2);
    }

    [Fact]
    public void Equals_WithDifferentInstances_ShouldReturnFalse()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.False(volume1.Equals(volume2));
        Assert.False(volume1 == volume2);
        Assert.True(volume1 != volume2);
    }

    [Fact]
    public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(volume1.GetHashCode(), volume2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.NotEqual(volume1.GetHashCode(), volume2.GetHashCode());
    }

    [Fact]
    public void Greater_WithGreaterInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(2, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1 > volume2);
    }

    [Fact]
    public void Greater_WithEqualInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.False(volume1 > volume2);
    }

    [Fact]
    public void Greater_WithSmallerInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.False(volume1 > volume2);
    }

    [Fact]
    public void GreaterOrEqual_WithGreaterInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(2, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1 >= volume2);
    }

    [Fact]
    public void GreaterOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1 >= volume2);
    }

    [Fact]
    public void GreaterOrEqual_WithSmallerInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.False(volume1 >= volume2);
    }

    [Fact]
    public void Less_WithGreaterInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(2, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.False(volume1 < volume2);
    }

    [Fact]
    public void Less_WithEqualInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.False(volume1 < volume2);
    }

    [Fact]
    public void Less_WithSmallerInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.True(volume1 < volume2);
    }

    [Fact]
    public void LessOrEqual_WithGreaterInstance_ShouldReturnFalse()
    {
        var volume1 = new Volume(2, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.False(volume1 <= volume2);
    }

    [Fact]
    public void LessOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1 <= volume2);
    }

    [Fact]
    public void LessOrEqual_WithSmallerInstance_ShouldReturnTrue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.True(volume1 <= volume2);
    }

    [Fact]
    public void Add_WithSameUnit_ShouldReturnCorrectValue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        var result = volume1 + volume2;
        Assert.Equal(3, result.CubicMeters, 0.00001);
    }

    [Fact]
    public void Add_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(36, VolumeUnit.CubicFeet);
        var result = volume1 + volume2;
        Assert.Equal(2.01941, result.CubicMeters, 0.00001);
    }

    [Fact]
    public void Subtract_WithSameUnit_ShouldReturnCorrectValue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        var result = volume2 - volume1;
        Assert.Equal(1, result.CubicMeters, 0.00001);
    }

    [Fact]
    public void Subtract_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(141.259, VolumeUnit.CubicFeet);
        var result = volume2 - volume1;
        Assert.Equal(3, result.CubicMeters, 0.00001);
    }

    [Fact]
    public void CompareTo_WithGreaterInstance_ShouldReturnPositive()
    {
        var volume1 = new Volume(2, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1.CompareTo(volume2) > 0);
    }

    [Fact]
    public void CompareTo_WithEqualInstance_ShouldReturnZero()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(0, volume1.CompareTo(volume2));
    }

    [Fact]
    public void CompareTo_WithSmallerInstance_ShouldReturnNegative()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        var volume2 = new Volume(2, VolumeUnit.CubicMeter);
        Assert.True(volume1.CompareTo(volume2) < 0);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        var volume1 = new Volume(1, VolumeUnit.CubicMeter);
        Assert.True(volume1.CompareTo(null) > 0);
    }

    [Fact]
    public void CubicCentimeters_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(1000000, volume.CubicCentimeters, 0.00001);
    }

    [Fact]
    public void CubicMeters_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(1, volume.CubicMeters, 0.00001);
    }

    [Fact]
    public void CubicFeet_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(35.3146667215, volume.CubicFeet, 0.0001);
    }

    [Fact]
    public void Liters_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(1000, volume.Liters, 0.00001);
    }

    [Fact]
    public void Milliliters_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(1000000, volume.Milliliters, 0.00001);
    }

    [Fact]
    public void Gallons_WithOneCubicMeter_ShouldReturnCorrectValue()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(264.172052358, volume.Gallons, 0.001);
    }

    [Theory]
    [InlineData("cm³", "cm³")]
    [InlineData("cm3", "cm³")]
    [InlineData("CM3", "cm³")]
    [InlineData("Cubic Centimeter", "cm³")]
    [InlineData("m³", "m³")]
    [InlineData("m3", "m³")]
    [InlineData("M3", "m³")]
    [InlineData("Cubic Meter", "m³")]
    [InlineData("ft³", "ft³")]
    [InlineData("ft3", "ft³")]
    [InlineData("FT3", "ft³")]
    [InlineData("Cubic Feet", "ft³")]
    [InlineData("l", "l")]
    [InlineData("L", "l")]
    [InlineData("Liter", "l")]
    [InlineData("ml", "ml")]
    [InlineData("ML", "ml")]
    [InlineData("Milliliter", "ml")]
    [InlineData("gal", "gal")]
    [InlineData("GAL", "gal")]
    [InlineData("Gallon", "gal")]
    public void ToString_WithValidFormat_ReturnsCorrectUnit(string fmt, string expectedUnit)
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        var result = volume.ToString(fmt);
        Assert.Contains(expectedUnit, result);
    }

    [Fact]
    public void ToString_WithInvalidFormat_ShouldThrowException()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Throws<FormatException>(() => volume.ToString("invalid"));
    }

    [Fact]
    public void SetVolume_NegativeValue_ShouldSetToZero()
    {
        var volume = new Volume
        {
            CubicCentimeters = -1
        };

        Assert.Equal(0, volume.CubicCentimeters);

        volume.CubicMeters = -1;
        Assert.Equal(0, volume.CubicMeters);

        volume.CubicFeet = -1;
        Assert.Equal(0, volume.CubicFeet);

        volume.Liters = -1;
        Assert.Equal(0, volume.Liters);

        volume.Milliliters = -1;
        Assert.Equal(0, volume.Milliliters);

        volume.Gallons = -1;
        Assert.Equal(0, volume.Gallons);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="VolumeConverter"/> to be used when serializing and deserializing <see cref="Volume"/> objects.
    /// </summary>
    [Fact]
    public void Json_Serialize_ShouldReturnCorrectJson()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        var json = JsonSerializer.Serialize(volume, _options);
        output.WriteLine($"{nameof(Json_Serialize_ShouldReturnCorrectJson)}: serialized JSON: {json}");

        Assert.Equal("{\"value\":1,\"unit\":\"m3\"}", json, true, true, true, true);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="VolumeConverter"/> to be used when serializing and deserializing <see cref="Volume"/> objects.
    /// </summary>
    [Fact]
    public void Json_Deserialize_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1,\"unit\":\"m3\"}";
        var volume = JsonSerializer.Deserialize<Volume>(json, _options);

        var expected = new Volume(1, VolumeUnit.CubicMeter);
        Assert.Equal(expected, volume);
    }
}

public class VolumeConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Write_WithVolume_ShouldReturnCorrectJson()
    {
        var volume = new Volume(1, VolumeUnit.CubicMeter);
        var json = JsonSerializer.Serialize(volume, _options);

        Assert.Equal("{\"value\":1,\"unit\":\"m3\"}", json, true, true, true, true);
    }

    [Fact]
    public void Read_WithVolumeUnitFeet_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1,\"unit\":\"ft3\"}";
        var volume = JsonSerializer.Deserialize<Volume>(json, _options);

        var expected = new Volume(1, VolumeUnit.CubicFeet);
        Assert.Equal(expected, volume);
    }
}
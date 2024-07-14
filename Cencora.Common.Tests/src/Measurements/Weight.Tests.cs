// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Cencora.Common.Measurements;
using Xunit.Abstractions;

namespace Cencora.Common.Tests.Measurements;

public class WeightUnitExtensionsTests
{
    [Theory]
    [InlineData(WeightUnit.Microgram, "µg")]
    [InlineData(WeightUnit.Milligram, "mg")]
    [InlineData(WeightUnit.Gram, "g")]
    [InlineData(WeightUnit.Kilogram, "kg")]
    [InlineData(WeightUnit.Ton, "t")]
    [InlineData(WeightUnit.Pound, "lb")]
    [InlineData(WeightUnit.Ounce, "oz")]
    [InlineData(WeightUnit.Stone, "st.")]
    [InlineData(WeightUnit.Carat, "ct")]
    [InlineData(WeightUnit.LongTon, "lt")]
    [InlineData(WeightUnit.ShortTon, "st")]
    public void ToUnitString_WithDistanceUnit_ShouldReturnString(WeightUnit unit, string expected)
    {
        var result = unit.ToUnitString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToUnitString_WithInvalidWeightUnit_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => ((WeightUnit)int.MaxValue).ToUnitString());
    }

    [Theory]
    [InlineData("µg", WeightUnit.Microgram)]
    [InlineData("microgram", WeightUnit.Microgram)]
    [InlineData("micrograms", WeightUnit.Microgram)]
    [InlineData("mg", WeightUnit.Milligram)]
    [InlineData("milligram", WeightUnit.Milligram)]
    [InlineData("milligrams", WeightUnit.Milligram)]
    [InlineData("g", WeightUnit.Gram)]
    [InlineData("gram", WeightUnit.Gram)]
    [InlineData("grams", WeightUnit.Gram)]
    [InlineData("kg", WeightUnit.Kilogram)]
    [InlineData("kilogram", WeightUnit.Kilogram)]
    [InlineData("kilograms", WeightUnit.Kilogram)]
    [InlineData("t", WeightUnit.Ton)]
    [InlineData("ton", WeightUnit.Ton)]
    [InlineData("tons", WeightUnit.Ton)]
    [InlineData("lb", WeightUnit.Pound)]
    [InlineData("pound", WeightUnit.Pound)]
    [InlineData("pounds", WeightUnit.Pound)]
    [InlineData("oz", WeightUnit.Ounce)]
    [InlineData("ounce", WeightUnit.Ounce)]
    [InlineData("ounces", WeightUnit.Ounce)]
    [InlineData("st.", WeightUnit.Stone)]
    [InlineData("stone", WeightUnit.Stone)]
    [InlineData("stones", WeightUnit.Stone)]
    [InlineData("ct", WeightUnit.Carat)]
    [InlineData("carat", WeightUnit.Carat)]
    [InlineData("carats", WeightUnit.Carat)]
    [InlineData("lt", WeightUnit.LongTon)]  
    [InlineData("longton", WeightUnit.LongTon)]
    [InlineData("longtons", WeightUnit.LongTon)]
    [InlineData("Long Tons", WeightUnit.LongTon)]
    [InlineData("st", WeightUnit.ShortTon)]
    [InlineData("shortton", WeightUnit.ShortTon)]
    [InlineData("shorttons", WeightUnit.ShortTon)]
    [InlineData("Short Tons", WeightUnit.ShortTon)]
    public void FromString_WithValidString_ShouldReturnWeightUnit(string input, WeightUnit expected)
    {
        var result = WeightUnitExtensions.FromString(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FromString_WithInvalidString_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => WeightUnitExtensions.FromString("invalid"));
    }

    [Theory]
    [InlineData("µg")]
    [InlineData("microgram")]
    [InlineData("micrograms")]
    [InlineData("mg")]
    [InlineData("milligram")]
    [InlineData("milligrams")]
    [InlineData("g")]
    [InlineData("gram")]
    [InlineData("grams")]
    [InlineData("kg")]
    [InlineData("kilogram")]
    [InlineData("kilograms")]
    [InlineData("t")]
    [InlineData("ton")]
    [InlineData("tons")]
    [InlineData("lb")]
    [InlineData("pound")]
    [InlineData("pounds")]
    [InlineData("oz")]
    [InlineData("ounce")]
    [InlineData("ounces")]
    [InlineData("ct")]
    [InlineData("carat")]
    [InlineData("carats")]
    [InlineData("lt")]
    [InlineData("longton")]
    [InlineData("longtons")]
    [InlineData("st")]
    [InlineData("shortton")]
    [InlineData("shorttons")]
    public void IsValidUnitString_WithValidFormat_ShouldReturnTrue(string input)
    {
        var result = WeightUnitExtensions.IsValidUnitString(input);
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitString_WithInvalidFormat_ShouldReturnFalse()
    {
        var result = WeightUnitExtensions.IsValidUnitString("invalid");
        Assert.False(result);
    }
}
public class WeightTests(ITestOutputHelper output)
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        var weight = new Weight();
        Assert.Equal(0, weight.Kilograms);
    }

    [Theory]
    [InlineData(1, WeightUnit.Microgram, 0.000001)]
    [InlineData(1, WeightUnit.Milligram, 0.001)]
    [InlineData(1, WeightUnit.Gram, 1)]
    [InlineData(1, WeightUnit.Kilogram, 1000)]
    [InlineData(1, WeightUnit.Ton, 1000000)]
    [InlineData(1, WeightUnit.Pound, 453.59237)]
    [InlineData(1, WeightUnit.Ounce, 28.3495231)]
    [InlineData(1, WeightUnit.Stone, 6350.29318)]
    [InlineData(1, WeightUnit.Carat, 0.2)]
    [InlineData(1, WeightUnit.LongTon, 1016046.91)]
    [InlineData(1, WeightUnit.ShortTon, 907184.74)]
    public void Constructor_WithValidParameters_ShouldCreateInstance(double value, WeightUnit unit, double expectedGrams)
    {
        var weight = new Weight(value, unit);
        Assert.Equal(expectedGrams, weight.Grams, 0.01);
    }

    [Fact]
    public void Constructor_WithInvalidParameters_ShouldSetToZero()
    {
        var weight = new Weight(-1, WeightUnit.Gram);
        Assert.Equal(0, weight.Grams);

        weight = new Weight(-1, WeightUnit.Kilogram);
        Assert.Equal(0, weight.Kilograms);
    }

    [Fact]
    public void Equals_WithEqualInstances_ShouldReturnTrue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1.Equals(weight2));
        Assert.True(weight1 == weight2);
        Assert.False(weight1 != weight2);
    }

    [Fact]
    public void Equals_WithDifferentInstances_ShouldReturnFalse()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.False(weight1.Equals(weight2));
        Assert.False(weight1 == weight2);
        Assert.True(weight1 != weight2);
    }

    [Fact]
    public void Greater_WithGreaterInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1 > weight2);
    }

    [Fact]
    public void Greater_WithEqualInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.False(weight1 > weight2);
    }

    [Fact]
    public void Greater_WithSmallerInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.False(weight1 > weight2);
    }

    [Fact]
    public void GreaterOrEqual_WithGreaterInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1 >= weight2);
    }

    [Fact]
    public void GreaterOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1 >= weight2);
    }

    [Fact]
    public void GreaterOrEqual_WithSmallerInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.False(weight1 >= weight2);
    }

    [Fact]
    public void Less_WithGreaterInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.False(weight1 < weight2);
    }

    [Fact]
    public void Less_WithEqualInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.False(weight1 < weight2);
    }

    [Fact]
    public void Less_WithSmallerInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.True(weight1 < weight2);
    }

    [Fact]
    public void LessOrEqual_WithGreaterInstance_ShouldReturnFalse()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.False(weight1 <= weight2);
    }

    [Fact]
    public void LessOrEqual_WithEqualInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1 <= weight2);
    }

    [Fact]
    public void LessOrEqual_WithSmallerInstance_ShouldReturnTrue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.True(weight1 <= weight2);
    }

    [Fact]
    public void Add_WithSameUnit_ShouldReturnCorrectValue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        var result = weight1 + weight2;
        Assert.Equal(3, result.Grams, 0.01);
    }
        
    [Fact]
    public void Add_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Kilogram);
        var result = weight1 + weight2;
        Assert.Equal(2001, result.Grams, 0.01);

        weight1 = new Weight(1, WeightUnit.Kilogram);
        weight2 = new Weight(2, WeightUnit.Pound);
        result = weight1 + weight2;
        Assert.Equal(1907.185, result.Grams, 0.01);
    }

    [Fact]
    public void Subtract_WithSameUnit_ShouldReturnCorrectValue()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        var result = weight1 - weight2;
        Assert.Equal(1, result.Grams, 0.01);
    }

    [Fact]
    public void Subtract_WithDifferentUnit_ShouldReturnCorrectValue()
    {
        var weight1 = new Weight(2, WeightUnit.Kilogram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        var result = weight1 - weight2;
        Assert.Equal(1999, result.Grams, 0.01);

        weight1 = new Weight(1, WeightUnit.Kilogram);
        weight2 = new Weight(2, WeightUnit.Pound);
        result = weight1 - weight2;
        Assert.Equal(92.815, result.Grams, 0.01);
    }


    [Fact]
    public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.Equal(weight1.GetHashCode(), weight2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.NotEqual(weight1.GetHashCode(), weight2.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithEqualInstances_ShouldReturnZero()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0, weight1.CompareTo(weight2));
    }

    [Fact]
    public void CompareTo_WithGreaterInstance_ShouldReturnPositive()
    {
        var weight1 = new Weight(2, WeightUnit.Gram);
        var weight2 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1.CompareTo(weight2) > 0);
    }

    [Fact]
    public void CompareTo_WithSmallerInstance_ShouldReturnNegative()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        var weight2 = new Weight(2, WeightUnit.Gram);
        Assert.True(weight1.CompareTo(weight2) < 0);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        var weight1 = new Weight(1, WeightUnit.Gram);
        Assert.True(weight1.CompareTo(null) > 0);
    }

    [Fact]
    public void Micrograms_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(1000000, weight.Micrograms, 0.01);
    }

    [Fact]
    public void Milligrams_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(1000, weight.Milligrams,0.01);
    }

    [Fact]
    public void Grams_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(1, weight.Grams, 0.01);
    }

    [Fact]
    public void Kilograms_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.001, weight.Kilograms, 0.01);
    }

    [Fact]
    public void Tons_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.000001, weight.Tons, 0.01);
    }

    [Fact]
    public void Pounds_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.00220462, weight.Pounds, 0.01);
    }

    [Fact]
    public void Ounces_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.03527396, weight.Ounces, 0.01);
    }

    [Fact]
    public void Stone_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.000157473, weight.Stones, 0.01);
    }

    [Fact]
    public void Carats_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(5, weight.Carats, 0.01);
    }

    [Fact]
    public void LongTons_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.00000098420652761, weight.LongTons, 0.01);
    }

    [Fact]
    public void ShortTons_WithOneGram_ShouldReturnCorrectValue()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Equal(0.00000110231131, weight.ShortTons, 0.01);
    }

    [Theory]
    [InlineData("µg", "µg")]
    [InlineData("microgram", "µg")]
    [InlineData("micrograms", "µg")]
    [InlineData("mg", "mg")]
    [InlineData("milligram", "mg")]
    [InlineData("milligrams", "mg")]
    [InlineData("g", "g")]
    [InlineData("gram", "g")]
    [InlineData("grams", "g")]
    [InlineData("kg", "kg")]
    [InlineData("kilogram", "kg")]
    [InlineData("kilograms", "kg")]
    [InlineData("t", "t")]
    [InlineData("ton", "t")]
    [InlineData("tons", "t")]
    [InlineData("lb", "lb")]
    [InlineData("pound", "lb")]
    [InlineData("pounds", "lb")]
    [InlineData("oz", "oz")]
    [InlineData("ounce", "oz")]
    [InlineData("ounces", "oz")]
    [InlineData("st.", "st.")]
    [InlineData("stone", "st.")]
    [InlineData("stones", "st.")]
    [InlineData("ct", "ct")]
    [InlineData("carat", "ct")]
    [InlineData("carats", "ct")]
    [InlineData("lt", "lt")]
    [InlineData("longton", "lt")]
    [InlineData("longtons", "lt")]
    [InlineData("Long Tons", "lt")]
    [InlineData("st", "st")]
    [InlineData("shortton", "st")]
    [InlineData("shorttons", "st")]
    [InlineData("Short Tons", "st")]
    public void ToString_WithValidFormat_ReturnsCorrectUnit(string fmt, string expectedUnit)
    {
        var weight = new Weight(1, WeightUnit.Gram);
        var result = weight.ToString(fmt);
        Assert.EndsWith(expectedUnit, result);
    }

    [Fact]
    public void ToString_WithInvalidFormat_ShouldThrowException()
    {
        var weight = new Weight(1, WeightUnit.Gram);
        Assert.Throws<FormatException>(() => weight.ToString("invalid"));
    }

    [Fact]
    public void SetWeight_NegativeValue_ShouldSetToZero()
    {
        var weight = new Weight
        {
            Micrograms = -1
        };

        Assert.Equal(0, weight.Micrograms);

        weight.Milligrams = -1;
        Assert.Equal(0, weight.Milligrams);

        weight.Grams = -1;
        Assert.Equal(0, weight.Grams);

        weight.Kilograms = -1;
        Assert.Equal(0, weight.Kilograms);

        weight.Tons = -1;
        Assert.Equal(0, weight.Tons);

        weight.Pounds = -1;
        Assert.Equal(0, weight.Pounds);

        weight.Ounces = -1;
        Assert.Equal(0, weight.Ounces);

        weight.Stones = -1;
        Assert.Equal(0, weight.Stones);

        weight.Carats = -1;
        Assert.Equal(0, weight.Carats);

        weight.LongTons = -1;
        Assert.Equal(0, weight.LongTons);

        weight.ShortTons = -1;
        Assert.Equal(0, weight.ShortTons);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="WeightConverter"/> to be used when serializing and deserializing <see cref="Weight"/> objects.
    /// </summary>
    [Fact]
    public void Json_Serialize_ShouldReturnCorrectJson()
    {
        var weight = new Weight(1, WeightUnit.Kilogram);
        var json = JsonSerializer.Serialize(weight, _options);
        output.WriteLine($"{nameof(Json_Serialize_ShouldReturnCorrectJson)}: serialized JSON: {json}");

        Assert.Equal("{\"value\":1000,\"unit\":\"g\"}", json, true, true, true, true);
    }

    /// <summary>
    /// Test for the JsonConverter implementation.
    /// We want the <see cref="WeightConverter"/> to be used when serializing and deserializing <see cref="Weight"/> objects.
    /// </summary>
    [Fact]
    public void Json_Deserialize_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1,\"unit\":\"g\"}";
        var weight = JsonSerializer.Deserialize<Weight>(json, _options);

        var expected = new Weight(1, WeightUnit.Gram);
        Assert.Equal(expected, weight);
    }
}

public class WeightConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    [Fact]
    public void Write_WithDistance_ShouldReturnCorrectJson()
    {
        
        var weight = new Weight(1, WeightUnit.Kilogram);
        var json = JsonSerializer.Serialize(weight, _options);

        Assert.Equal("{\"value\":1000,\"unit\":\"g\"}", json, true, true, true, true);
    }

    [Fact]
    public void Read_WithDistanceUnitMillimeters_ShouldReturnCorrectInstance()
    {
        const string json = "{\"value\":1000,\"unit\":\"kg\"}";
        var weight = JsonSerializer.Deserialize<Weight>(json, _options);

        var expected = new Weight(1000, WeightUnit.Kilogram);
        Assert.Equal(expected, weight);
    }
}
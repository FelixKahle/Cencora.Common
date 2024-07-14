// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Cencora.Common.Measurements;
using Xunit.Abstractions;

namespace Cencora.Common.Tests.Measurements
{
    public class TemperatureUnitExtensionsTests
    {
        [Theory]
        [InlineData(TemperatureUnit.Celsius, "°C")]
        [InlineData(TemperatureUnit.Fahrenheit, "°F")]
        [InlineData(TemperatureUnit.Kelvin, "K")]
        public void ToUnitString_WithTemperatureUnit_ShouldReturnString(TemperatureUnit unit, string expected)
        {
            var result = unit.ToUnitString();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToUnitString_WithInvalidTemperatureUnit_ShouldThrowException()
        {
            var unit = (TemperatureUnit) 42;
            Assert.Throws<ArgumentException>(() => unit.ToUnitString());
        }

        [Theory]
        [InlineData("°C", TemperatureUnit.Celsius)]
        [InlineData("C", TemperatureUnit.Celsius)]
        [InlineData("c", TemperatureUnit.Celsius)]
        [InlineData("celsius", TemperatureUnit.Celsius)]
        [InlineData("°celsius", TemperatureUnit.Celsius)]
        [InlineData("°F", TemperatureUnit.Fahrenheit)]
        [InlineData("F", TemperatureUnit.Fahrenheit)]
        [InlineData("f", TemperatureUnit.Fahrenheit)]
        [InlineData("fahrenheit", TemperatureUnit.Fahrenheit)]
        [InlineData("°fahrenheit", TemperatureUnit.Fahrenheit)]
        [InlineData("K", TemperatureUnit.Kelvin)]
        [InlineData("k", TemperatureUnit.Kelvin)]
        [InlineData("kelvin", TemperatureUnit.Kelvin)]
        public void FromString_WithValidString_ShouldReturnTemperatureUnit(string input, TemperatureUnit expected)
        {
            var unit = TemperatureUnitExtensions.FromString(input);
            Assert.Equal(expected, unit);
        }

        [Fact]
        public void FromString_WithInvalidString_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => TemperatureUnitExtensions.FromString("invalid"));
        }

        [Theory]
        [InlineData("°C")]
        [InlineData("C")]
        [InlineData("c")]
        [InlineData("celsius")]
        [InlineData("°celsius")]
        [InlineData("°F")]
        [InlineData("F")]
        [InlineData("f")]
        [InlineData("fahrenheit")]
        [InlineData("°fahrenheit")]
        [InlineData("K")]
        [InlineData("k")]
        [InlineData("kelvin")]
        public void IsValidUnitString_WithValidFormat_ShouldReturnTrue(string input)
        {
            var result = TemperatureUnitExtensions.IsValidUnitString(input);
            Assert.True(result);
        }

        [Fact]
        public void IsValidUnitString_WithInvalidFormat_ShouldReturnFalse()
        {
            var result = TemperatureUnitExtensions.IsValidUnitString("invalid");
            Assert.False(result);
        }
    }

    public class TemperatureTests(ITestOutputHelper output)
    {
        [Fact]
        public void Constructor_WithoutParameters_ShouldCreateInstance()
        {
            var temperature = new Temperature();
            Assert.Equal(0, temperature.Kelvin);
        }

        [Theory]
        [InlineData(0, TemperatureUnit.Celsius, 273.15)]
        [InlineData(0, TemperatureUnit.Fahrenheit, 255.3722)]
        [InlineData(0, TemperatureUnit.Kelvin, 0)]
        public void Constructor_WithValidParameters_ShouldCreateInstance(double value, TemperatureUnit unit, double expectedKelvin)
        {
            var temperature = new Temperature(value, unit);
            Assert.Equal(expectedKelvin, temperature.Kelvin, 0.0001);
        }

        [Fact]
        public void Constructor_WithInvalidParameters_ShouldSetToZer()
        {
            var temperature = new Temperature(-1, TemperatureUnit.Kelvin);
            Assert.Equal(0, temperature.Kelvin);

            temperature = new Temperature(-300, TemperatureUnit.Celsius);
            Assert.Equal(0, temperature.Kelvin);
        }

        [Fact]
        public void Equals_WithEqualInstances_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.True(temperature1.Equals(temperature2));
            Assert.True(temperature1 == temperature2);
            Assert.False(temperature1 != temperature2);
        }

        [Fact]
        public void Equals_WithDifferentInstances_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Fahrenheit);
            Assert.False(temperature1.Equals(temperature2));
            Assert.False(temperature1 == temperature2);
            Assert.True(temperature1 != temperature2);
        }

        [Fact]
        public void Greater_WithGreaterInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(-1, TemperatureUnit.Celsius);
            Assert.True(temperature1 > temperature2);
        }

        [Fact]
        public void Greater_WithEqualInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.False(temperature1 > temperature2);
        }

        [Fact]
        public void Greater_WithSmallerInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(1, TemperatureUnit.Celsius);
            Assert.False(temperature1 > temperature2);
        }

        [Fact]
        public void GreaterOrEqual_WithGreaterInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(-1, TemperatureUnit.Celsius);
            Assert.True(temperature1 >= temperature2);
        }

        [Fact]
        public void GreaterOrEqual_WithEqualInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.True(temperature1 >= temperature2);
        }

        [Fact]
        public void GreaterOrEqual_WithSmallerInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(1, TemperatureUnit.Celsius);
            Assert.False(temperature1 >= temperature2);
        }

        [Fact]
        public void Less_WithGreaterInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(-1, TemperatureUnit.Celsius);
            Assert.False(temperature1 < temperature2);
        }

        [Fact]
        public void Less_WithEqualInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.False(temperature1 < temperature2);
        }

        [Fact]
        public void Less_WithSmallerInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(1, TemperatureUnit.Celsius);
            Assert.True(temperature1 < temperature2);
        }

        [Fact]
        public void LessOrEqual_WithGreaterInstance_ShouldReturnFalse()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(-1, TemperatureUnit.Celsius);
            Assert.False(temperature1 <= temperature2);
        }

        [Fact]
        public void LessOrEqual_WithEqualInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.True(temperature1 <= temperature2);
        }

        [Fact]
        public void LessOrEqual_WithSmallerInstance_ShouldReturnTrue()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(1, TemperatureUnit.Celsius);
            Assert.True(temperature1 <= temperature2);
        }

        [Fact]
        public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.Equal(temperature1.GetHashCode(), temperature2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Fahrenheit);
            Assert.NotEqual(temperature1.GetHashCode(), temperature2.GetHashCode());
        }

        [Fact]
        public void CompareTo_WithEqualInstances_ShouldReturnZero()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.Equal(0, temperature1.CompareTo(temperature2));
        }

        [Fact]
        public void CompareTo_WithGreaterInstance_ShouldReturnPositive()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(-1, TemperatureUnit.Celsius);
            Assert.True(temperature1.CompareTo(temperature2) > 0);
        }

        [Fact]
        public void CompareTo_WithSmallerInstance_ShouldReturnNegative()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(1, TemperatureUnit.Celsius);
            Assert.True(temperature1.CompareTo(temperature2) < 0);
        }

        [Fact]
        public void CompareTo_WithNull_ShouldReturnPositive()
        {
            var temperature1 = new Temperature(0, TemperatureUnit.Celsius);
            Assert.True(temperature1.CompareTo(null) > 0);
        }

        [Fact]
        public void Kelvin_WithTenKelvin_ShouldReturnCorrectValue()
        {
            var temperature = new Temperature(10, TemperatureUnit.Kelvin);
            Assert.Equal(10, temperature.Kelvin, 0.0001);
        }

        [Fact]
        public void Celsius_WithTenKelvin_ShouldReturnCorrectValue()
        {
            var temperature = new Temperature(10, TemperatureUnit.Kelvin);
            Assert.Equal(-263.15, temperature.Celsius, 0.0001);
        }

        [Fact]
        public void Fahrenheit_WithTenKelvin_ShouldReturnCorrectValue()
        {
            var temperature = new Temperature(10, TemperatureUnit.Kelvin);
            Assert.Equal(-441.67, temperature.Fahrenheit, 0.0001);
        }

        [Theory]
        [InlineData("K", "K")]
        [InlineData("Kelvin", "K")]
        [InlineData("°K", "K")]
        [InlineData("°Kelvin", "K")]
        [InlineData("C", "°C")]
        [InlineData("Celsius", "°C")]
        [InlineData("°Celsius", "°C")]
        [InlineData("F", "°F")]
        [InlineData("Fahrenheit", "°F")]
        [InlineData("°Fahrenheit", "°F")]
        public void ToString_WithValidFormat_ReturnsCorrectUnit(string fmt, string expectedUnit)
        {
            var temperature = new Temperature(10, TemperatureUnit.Kelvin);
            var result = temperature.ToString(fmt);
            Assert.Contains(expectedUnit, result);
        }

        [Fact]
        public void ToString_WithInvalidFormat_ShouldThrowException()
        {
            var temperature = new Temperature(10, TemperatureUnit.Kelvin);
            Assert.Throws<FormatException>(() => temperature.ToString("invalid"));
        }

        [Fact]
        public void SetTemperature_WithInvalidValue_ShouldSetToZero()
        {
            var temperature = new Temperature();

            temperature.Kelvin = -1;
            Assert.Equal(0, temperature.Kelvin);

            temperature.Celsius = -274;
            Assert.Equal(0, temperature.Kelvin);

            temperature.Fahrenheit = -460;
            Assert.Equal(0, temperature.Kelvin);
        }

        /// <summary>
        /// Test for the JsonConverter implementation.
        /// We want the <see cref="TemperatureConverter"/> to be used when serializing and deserializing <see cref="Temperature"/> objects.
        /// </summary>
        [Fact]
        public void Json_Serialize_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var temperature = new Temperature(0, TemperatureUnit.Celsius);
            string json = JsonSerializer.Serialize(temperature, options);
            output.WriteLine($"{nameof(Json_Serialize_ShouldReturnCorrectJson)}: serialized JSON: {json}");

            Assert.Equal("{\"value\":273.15,\"unit\":\"k\"}", json, true, true, true, true);
        }

        /// <summary>
        /// Test for the JsonConverter implementation.
        /// We want the <see cref="TemperatureConverter"/> to be used when serializing and deserializing <see cref="Temperature"/> objects.
        /// </summary>
        [Fact]
        public void Json_Deserialize_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = "{\"value\":273.15,\"unit\":\"k\"}";
            var temperature = JsonSerializer.Deserialize<Temperature>(json, options);

            var expected = new Temperature(0, TemperatureUnit.Celsius);
            Assert.Equal(expected, temperature);
        }
    }

    public class TemperatureConverterTests
    {
        [Fact]
        public void Write_WithTemperature_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var temperature = new Temperature(0, TemperatureUnit.Celsius);
            string json = JsonSerializer.Serialize(temperature, options);

            Assert.Equal("{\"value\":273.15,\"unit\":\"k\"}", json, true, true, true, true);
        }

        [Fact]
        public void Read_WithTemperatureUnitCelsius_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = "{\"value\":10,\"unit\":\"°C\"}";
            var temperature = JsonSerializer.Deserialize<Temperature>(json, options);

            var expected = new Temperature(10, TemperatureUnit.Celsius);
            Assert.Equal(expected, temperature);
        }
    }
}
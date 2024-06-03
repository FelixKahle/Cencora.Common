// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class TemperatureTests
    {
        private readonly ITestOutputHelper output;

        public TemperatureTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Temperature_Constructor_InitializesToZero()
        {
            var temperature = new Temperature();
            Assert.Equal(0, temperature.Kelvin);
        }

        [Fact]
        public void Temperature_Constructor_InitializesCorrectly()
        {
            var temperature = new Temperature(2, TemperatureUnit.Celsius);
            Assert.Equal(2, temperature.Celsius);
        }

        [Fact]
        public void Temperature_Fahrenheit_ReturnsCorrectValue()
        {
            var temperature = new Temperature(2, TemperatureUnit.Celsius);
            Assert.Equal(35.6, temperature.Fahrenheit, 0.005);
        }

        [Fact]
        public void Temperature_Celsius_ReturnsCorrectValue()
        {
            var temperature = new Temperature(2, TemperatureUnit.Celsius);
            Assert.Equal(2, temperature.Celsius);
        }

        [Fact]
        public void Temperature_Kelvin_ReturnsCorrectValue()
        {
            var temperature = new Temperature(2, TemperatureUnit.Celsius);
            Assert.Equal(275.15, temperature.Kelvin, 0.005);
        }

        [Fact]
        public void Temperature_Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {
            var temperature1 = new Temperature(2, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(2, TemperatureUnit.Celsius);
            Assert.Equal(temperature1, temperature2);
            Assert.True(temperature1 == temperature2);
            Assert.False(temperature1 != temperature2);
        }

        [Fact]
        public void Temperature_Equals_ReturnsFalse_IfAnyPropertyIsDifferent()
        {
            var temperature1 = new Temperature(2, TemperatureUnit.Celsius);
            var temperature2 = new Temperature(2, TemperatureUnit.Fahrenheit);
            Assert.NotEqual(temperature1, temperature2);
            Assert.False(temperature1 == temperature2);
            Assert.True(temperature1 != temperature2);
        }

        [Fact]
        public void Temperature_JSON_SerializeAndDeserialize_Correctly()
        {
            var temperature = new Temperature(2, TemperatureUnit.Celsius);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(temperature, options);
            output.WriteLine(json);

            var deserialized = JsonSerializer.Deserialize<Temperature>(json, options);

            Assert.Equal(temperature, deserialized);
        }
    }
}
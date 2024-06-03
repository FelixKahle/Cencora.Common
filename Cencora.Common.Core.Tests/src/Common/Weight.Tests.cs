// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class WeightTests
    {
        private readonly ITestOutputHelper output;

        public WeightTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Weight_Constructor_InitializesToZero()
        {
            var weight = new Weight();
            Assert.Equal(0, weight.Kilograms);
        }

        [Fact]
        public void Weight_Constructor_InitializesCorrectly()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(2, weight.Kilograms);
        }

        [Fact]
        public void Weight_Pounds_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(4.40925, weight.Pounds, 0.0001);
        }

        [Fact]
        public void Weight_Ounces_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(70.5479, weight.Ounces, 0.0001);
        }

        [Fact]
        public void Weight_Grams_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(2000, weight.Grams);
        }

        [Fact]
        public void Weight_Milligrams_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(2000000, weight.Milligrams);
        }

        [Fact]
        public void Weight_Micrograms_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(2000000000, weight.Micrograms);
        }

        [Fact]
        public void Weight_Tons_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(0.002, weight.Tons);
        }

        [Fact]
        public void Weight_Kilograms_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(2, weight.Kilograms);
        }

        [Fact]
        public void Weight_Stones_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(0.31496, weight.Stones, 0.001);
        }

        [Fact]
        public void Weight_Carats_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(10000, weight.Carats);
        }

        [Fact]
        public void Weight_LongTons_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(0.00196841, weight.LongTons, 0.0001);
        }

        [Fact]
        public void Weight_ShortTons_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(0.00220462, weight.ShortTons, 0.0001);
        }

        [Fact]
        public void Weight_Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {
            var weight1 = new Weight(2, WeightUnit.Kilograms);
            var weight2 = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal(weight1, weight2);
            Assert.True(weight1 == weight2);
            Assert.False(weight1 != weight2);
        }

        [Fact]
        public void Weight_Equals_ReturnsFalse_IfAnyPropertyIsDifferent()
        {
            var weight1 = new Weight(2, WeightUnit.Kilograms);
            var weight2 = new Weight(2, WeightUnit.Pounds);
            Assert.NotEqual(weight1, weight2);
            Assert.False(weight1 == weight2);
            Assert.True(weight1 != weight2);
        }

        [Fact]
        public void Weight_ToString_ReturnsCorrectValue()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);
            Assert.Equal("2 kg", weight.ToString("KG", null));
        }

        [Fact]
        public void Weight_JSON_SerializeAndDeserialize_Correctly()
        {
            var weight = new Weight(2, WeightUnit.Kilograms);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(weight, options);
            output.WriteLine(json);

            var deserialized = JsonSerializer.Deserialize<Weight>(json, options);

            Assert.Equal(weight, deserialized);
        }
    }
}
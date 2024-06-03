// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;

namespace Cencora.Common.Core.Tests
{
    public class DistanceTests
    {
        [Fact]
        public void Distance_Constructor_InitializesToZero()
        {
            var distance = new Distance();
            Assert.Equal(0, distance.Meters);
        }

        [Fact]
        public void Distance_Constructor_InitializesCorrectly()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(2, distance.Meters);
        }

        [Fact]
        public void Distance_Millimeters_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(2000, distance.Millimeters);
        }

        [Fact]
        public void Distance_Centimeters_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(200, distance.Centimeters);
        }

        [Fact]
        public void Distance_Meters_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(2, distance.Meters);
        }

        [Fact]
        public void Distance_Kilometers_ReturnsCorrectValue()
        {
            var distance = new Distance(2000, DistanceUnit.Meters);
            Assert.Equal(2, distance.Kilometers);
        }

        [Fact]
        public void Distance_Inches_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(78.7402, distance.Inches, 0.005);
        }

        [Fact]
        public void Distance_Feet_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(6.56168, distance.Feet, 0.005);
        }

        [Fact]
        public void Distance_Yards_ReturnsCorrectValue()
        {
            var distance = new Distance(2, DistanceUnit.Meters);
            Assert.Equal(2.18722, distance.Yards, 0.005);
        }

        [Fact]
        public void Distance_Miles_ReturnsCorrectValue()
        {
            var distance = new Distance(2000, DistanceUnit.Meters);
            Assert.Equal(1.242742, distance.Miles, 0.005);
        }

        [Fact]
        public void Distance_NauticalMiles_ReturnsCorrectValue()
        {
            var distance = new Distance(2000, DistanceUnit.Meters);
            Assert.Equal(1.07991327278618, distance.NauticalMiles, 0.005);
        }

        [Fact]
        public void Distance_Addition_ReturnsCorrectValue()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var distance2 = new Distance(3, DistanceUnit.Meters);
            var result = distance1 + distance2;
            Assert.Equal(5, result.Meters);
        }

        [Fact]
        public void Distance_Subtraction_ReturnsCorrectValue()
        {
            var distance1 = new Distance(3, DistanceUnit.Meters);
            var distance2 = new Distance(2, DistanceUnit.Meters);
            var result = distance1 - distance2;
            Assert.Equal(1, result.Meters);
        }

        [Fact]
        public void Distance_Multiplication_ReturnsCorrectValue()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var result1 = distance1 * 2;
            Assert.Equal(4, result1.Meters);

            var distance2 = new Distance(3, DistanceUnit.Meters);
            var result2 = distance1 * distance2;
            Assert.Equal(6, result2.Meters);

            var result3 = 2 * distance1;
            Assert.Equal(4, result3.Meters);
        }

        [Fact]
        public void Distance_Division_ReturnsCorrectValue()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var result1 = distance1 / 2;
            Assert.Equal(1, result1.Meters);

            var distance2 = new Distance(6, DistanceUnit.Meters);
            var result2 = distance2 / distance1;
            Assert.Equal(3, result2);

            var result3 = 6 / distance1;
            Assert.Equal(3, result3.Meters);
        }

        [Fact]
        public void Distance_GreaterThan_ReturnsTrue()
        {
            var distance1 = new Distance(3, DistanceUnit.Meters);
            var distance2 = new Distance(2, DistanceUnit.Meters);
            Assert.True(distance1 > distance2);
        }

        [Fact]
        public void Distance_GreaterThan_ReturnsFalse()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var distance2 = new Distance(3, DistanceUnit.Meters);
            Assert.False(distance1 > distance2);
        }

        [Fact]
        public void Distance_LessThan_ReturnsTrue()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var distance2 = new Distance(3, DistanceUnit.Meters);
            Assert.True(distance1 < distance2);
        }

        [Fact]
        public void Distance_LessThan_ReturnsFalse()
        {
            var distance1 = new Distance(3, DistanceUnit.Meters);
            var distance2 = new Distance(2, DistanceUnit.Meters);
            Assert.False(distance1 < distance2);
        }

        [Fact]
        public void Distance_Equals_ReturnsTrue()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var distance2 = new Distance(2, DistanceUnit.Meters);
            Assert.True(distance1.Equals(distance2));
            Assert.True(distance1 == distance2);
            Assert.False(distance1 != distance2);
        }

        [Fact]
        public void Distance_Equals_ReturnsFalse()
        {
            var distance1 = new Distance(2, DistanceUnit.Meters);
            var distance2 = new Distance(3, DistanceUnit.Meters);
            Assert.False(distance1.Equals(distance2));
            Assert.False(distance1 == distance2);
            Assert.True(distance1 != distance2);
        }


        [Fact]
        public void Distance_JSON_SerializeAndDeserialize_Correctly()
        {
            var distance = new Distance(2, DistanceUnit.Meters);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(distance, options);
            Assert.False(string.IsNullOrEmpty(json));
            var deserializedDistance = JsonSerializer.Deserialize<Distance>(json, options);
            Assert.Equal(distance, deserializedDistance);
        }
    }
}
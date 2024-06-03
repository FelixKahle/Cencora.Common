// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class VolumeTests
    {
        private readonly ITestOutputHelper output;

        public VolumeTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Volume_Constructor_InitializesToZero()
        {
            var volume = new Volume();
            Assert.Equal(0, volume.CubicMeters);
        }

        [Fact]
        public void Volume_Constructor_InitializesCorrectly()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(2, volume.CubicMeters);
        }

        [Fact]
        public void Volume_CubicMeters_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(2, volume.CubicMeters);
        }

        [Fact]
        public void Volume_CubicCentimeters_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(2000000, volume.CubicCentimeters);
        }

        [Fact]
        public void Volume_Liters_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(2000, volume.Liters);
        }

        [Fact]
        public void Volume_Gallons_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(528.3441, volume.Gallons, 0.001);
        }

        [Fact]
        public void Volume_CubicFeet_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(70.6293, volume.CubicFeet, 0.001);
        }

        [Fact]
        public void Volume_Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {
            var volume1 = new Volume(2, VolumeUnit.CubicMeter);
            var volume2 = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal(volume1, volume2);
            Assert.True(volume1 == volume2);
            Assert.False(volume1 != volume2);
        }

        [Fact]
        public void Volume_Equals_ReturnsFalse_IfAnyPropertyIsDifferent()
        {
            var volume1 = new Volume(2, VolumeUnit.CubicMeter);
            var volume2 = new Volume(2, VolumeUnit.CubicCentimeter);
            Assert.NotEqual(volume1, volume2);
            Assert.False(volume1 == volume2);
            Assert.True(volume1 != volume2);
        }

        [Fact]
        public void Volume_ToString_ReturnsCorrectValue()
        {
            var volume = new Volume(2, VolumeUnit.CubicMeter);
            Assert.Equal("2 m³", volume.ToString("m3", null));
        }

        [Fact]
        public void Volume_JSON_SerializeAndDeserialize_Correctly()
        {
            var volume = new Volume(2, VolumeUnit.Liter);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(volume, options);
            output.WriteLine(json);

            var deserializedVolume = JsonSerializer.Deserialize<Volume>(json, options);
            Assert.Equal(volume, deserializedVolume);
        }
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class AddressTests
    {
        private readonly ITestOutputHelper output;

        public AddressTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Address_Constructor_InitializesCorrectly()
        {
            var address = new Address("Street", "Line2", "City", "12345", "State", "Country");
            Assert.Equal("Street", address.StreetAddress);
            Assert.Equal("Line2", address.AddressLine2);
            Assert.Equal("City", address.City);
            Assert.Equal("12345", address.PostalCode);
            Assert.Equal("State", address.StateOrProvince);
            Assert.Equal("Country", address.Country);
        }

        [Fact]
        public void Address_Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {
            var address1 = new Address("Street", "Line2", "City", "12345", "State", "Country");
            var address2 = new Address("Street", "Line2", "City", "12345", "State", "Country");
            Assert.Equal(address1, address2);
            Assert.True(address1 == address2);
            Assert.False(address1 != address2);
        }

        [Fact]
        public void Address_Equals_ReturnsFalse_IfAnyPropertyIsDifferent()
        {
            var address1 = new Address("Street", "Line2", "City", "12345", "State", "Country");
            var address2 = new Address("Street", "Line2", "City", "12345", "State", "Country2");
            Assert.NotEqual(address1, address2);
            Assert.False(address1 == address2);
            Assert.True(address1 != address2);
        }

        [Fact]
        public void Address_JSON_SerializeAndDeserialize_Correctly()
        {
            var address = new Address("Street", "Line2", "City", "12345", "State", "Country");

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(address, options);
            output.WriteLine(json);
            
            var deserialized = JsonSerializer.Deserialize<Address>(json, options);
            Assert.Equal(address, deserialized);
        }
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Maps;

namespace Cencora.Common.Tests.Maps
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_WithoutParameters_ShouldCreateInstance()
        {
            Address address = new Address();

            Assert.Equal(string.Empty, address.AddressLine1);
            Assert.Equal(string.Empty, address.AddressLine2);
            Assert.Equal(string.Empty, address.City);
            Assert.Equal(string.Empty, address.StateOrProvince);
            Assert.Equal(string.Empty, address.PostalCode);
            Assert.Equal(string.Empty, address.Country);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCreateInstance()
        {
            Address address = new Address
            {
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                City = "City",
                StateOrProvince = "StateOrProvince",
                PostalCode = "PostalCode",
                Country = "Country"
            };
            
            Assert.Equal("AddressLine1", address.AddressLine1);
            Assert.Equal("AddressLine2", address.AddressLine2);
            Assert.Equal("City", address.City);
            Assert.Equal("StateOrProvince", address.StateOrProvince);
            Assert.Equal("PostalCode", address.PostalCode);
            Assert.Equal("Country", address.Country);
        }
    }
}
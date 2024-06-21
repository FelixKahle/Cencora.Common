// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents an address.
    /// </summary>
    public struct Address : IEquatable<Address>
    {
        [JsonIgnore]
        private string _addressLine1 = string.Empty;
        [JsonIgnore]
        private string _addressLine2 = string.Empty;
        [JsonIgnore]
        private string _city = string.Empty;
        [JsonIgnore]
        private string _postalCode = string.Empty;
        [JsonIgnore]
        private string _stateOrProvince = string.Empty;
        [JsonIgnore]
        private string _country = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        public Address()
        {}

        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        [JsonInclude]
        public string AddressLine1
        {
            get => _addressLine1;
            set => _addressLine1 = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        [JsonInclude]
        public string AddressLine2
        {
            get => _addressLine2;
            set => _addressLine2 = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        [JsonInclude]
        public string City
        {
            get => _city;
            set => _city = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        [JsonInclude]
        public string PostalCode
        {
            get => _postalCode;
            set => _postalCode = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the state or province of the address.
        /// </summary>
        [JsonInclude]
        public string StateOrProvince
        {
            get => _stateOrProvince;
            set => _stateOrProvince = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        [JsonInclude]
        public string Country
        {
            get => _country;
            set => _country = value ?? string.Empty;
        }

        /// <inheritdoc/>
        public bool Equals(Address other)
        {
            return AddressLine1 == other.AddressLine1 &&
                   AddressLine2 == other.AddressLine2 &&
                   City == other.City &&
                   PostalCode == other.PostalCode &&
                   StateOrProvince == other.StateOrProvince &&
                   Country == other.Country;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Address other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(AddressLine1, AddressLine2, City, PostalCode, StateOrProvince, Country);
        }
    }
}
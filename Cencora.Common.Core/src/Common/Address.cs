// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a physical address.
    /// </summary>
    public struct Address : IEquatable<Address>
    {
        /// <summary>
        /// The street address.
        /// </summary>
        [JsonIgnore]
        private string _streetAddress;

        /// <summary>
        /// The address line 2.
        /// </summary>
        [JsonIgnore]
        private string _addressLine2;

        /// <summary>
        /// The city.
        /// </summary>
        [JsonIgnore]
        private string _city;

        /// <summary>
        /// The postal code.
        /// </summary>
        [JsonIgnore]
        private string _postalCode;

        /// <summary>
        /// The state or province.
        /// </summary>
        [JsonIgnore]
        private string _stateOrProvince;

        /// <summary>
        /// The country.
        /// </summary>
        [JsonIgnore]
        private string _country;

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string StreetAddress
        {
            get => _streetAddress;
            set => _streetAddress = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string AddressLine2
        {
            get => _addressLine2;
            set => _addressLine2 = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string City
        {
            get => _city;
            set => _city = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string PostalCode
        {
            get => _postalCode;
            set => _postalCode = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the state or province.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string StateOrProvince
        {
            get => _stateOrProvince;
            set => _stateOrProvince = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// </remarks>
        [JsonInclude]
        public string Country
        {
            get => _country;
            set => _country = value ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        public Address()
        {
            _streetAddress = string.Empty;
            _addressLine2 = string.Empty;
            _city = string.Empty;
            _postalCode = string.Empty;
            _stateOrProvince = string.Empty;
            _country = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="streetAddress">The street address.</param>
        /// <param name="addressLine2">The address line 2.</param>
        /// <param name="city">The city.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="stateOrProvince">The state or province.</param>
        /// <param name="country">The country.</param>
        [JsonConstructor]
        public Address(string streetAddress, string addressLine2, string city, string postalCode, string stateOrProvince, string country)
        {
            _streetAddress = streetAddress ?? string.Empty;
            _addressLine2 = addressLine2 ?? string.Empty;
            _city = city ?? string.Empty;
            _postalCode = postalCode ?? string.Empty;
            _stateOrProvince = stateOrProvince ?? string.Empty;
            _country = country ?? string.Empty;
        }

        /// <inheritdoc/>
        public bool Equals(Address other)
        {
            return StreetAddress.Equals(other.StreetAddress) &&
                   AddressLine2.Equals(other.AddressLine2) &&
                   City.Equals(other.City) &&
                   PostalCode.Equals(other.PostalCode) &&
                   StateOrProvince.Equals(other.StateOrProvince) &&
                   Country.Equals(other.Country);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Address other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(StreetAddress, AddressLine2, City, PostalCode, StateOrProvince, Country);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{StreetAddress}, {AddressLine2}, {City}, {PostalCode}, {StateOrProvince}, {Country}";
        }

        public static bool operator ==(Address left, Address right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !left.Equals(right);
        }
    }
}
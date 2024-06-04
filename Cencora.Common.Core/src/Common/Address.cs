// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a physical address.
    /// </summary>
    /// <remarks>
    /// Note that equality is not case-sensitive, i.e. "München" equals "münchen".
    /// This is because we are not interested in the casing of the address, 
    /// it should be treated as the same address.
    /// </remarks>
    public struct Address : IEquatable<Address>
    {
        /// <summary>
        /// The address line.
        /// </summary>
        [JsonIgnore]
        private string _addressLine;

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
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// The assignment normalizes the value to ensure that it is in a consistent format.
        /// </remarks>
        [JsonInclude]
        public string AddressLine
        {
            get => _addressLine;
            set => _addressLine = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// The assignment normalizes the value to ensure that it is in a consistent format.
        /// </remarks>
        [JsonInclude]
        public string AddressLine2
        {
            get => _addressLine2;
            set => _addressLine2 = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// The assignment normalizes the value to ensure that it is in a consistent format.
        /// </remarks>
        [JsonInclude]
        public string City
        {
            get => _city;
            set => _city = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// The assignment normalizes the value to ensure that it is in a consistent format.
        /// </remarks>
        [JsonInclude]
        public string PostalCode
        {
            get => _postalCode;
            set => _postalCode = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the state or province.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// The assignment normalizes the value to ensure that it is in a consistent format.
        /// </remarks>
        [JsonInclude]
        public string StateOrProvince
        {
            get => _stateOrProvince;
            set => _stateOrProvince = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// This property is never null. If the value is null, it is converted to an empty string.
        /// The assignment trims the value to remove leading and trailing white spaces.
        /// </remarks>
        [JsonInclude]
        public string Country
        {
            get => _country;
            set => _country = value?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Normalizes the address by converting all strings to uppercase.
        /// </summary>
        /// <returns>A new <see cref="Address"/> with all strings converted to uppercase.</returns>
        public Address Normalize()
        {
            return new Address(
                AddressLine.ToUpperInvariant(),
                AddressLine2.ToUpperInvariant(),
                City.ToUpperInvariant(),
                PostalCode.ToUpperInvariant(),
                StateOrProvince.ToUpperInvariant(),
                Country.ToUpperInvariant()
            );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        public Address()
        {
            _addressLine = string.Empty;
            _addressLine2 = string.Empty;
            _city = string.Empty;
            _postalCode = string.Empty;
            _stateOrProvince = string.Empty;
            _country = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="addressLine">The street address.</param>
        /// <param name="addressLine2">The address line 2.</param>
        /// <param name="city">The city.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="stateOrProvince">The state or province.</param>
        /// <param name="country">The country.</param>
        /// <remarks>
        /// The parameters are trimmed to remove leading and trailing white spaces.
        /// The parameters are normalized to ensure that they are in a consistent format.
        /// </remarks>
        [JsonConstructor]
        public Address(string addressLine, string addressLine2, string city, string postalCode, string stateOrProvince, string country)
        {
            _addressLine = addressLine?.Trim().Normalize() ?? string.Empty;
            _addressLine2 = addressLine2?.Trim().Normalize() ?? string.Empty;
            _city = city?.Trim().Normalize() ?? string.Empty;
            _postalCode = postalCode?.Trim().Normalize() ?? string.Empty;
            _stateOrProvince = stateOrProvince?.Trim() ?? string.Empty;
            _country = country?.Trim().Normalize() ?? string.Empty;
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="Address"/> are equal.
        /// </summary>
        /// <param name="other">The other <see cref="Address"/> to compare.</param>
        /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The comparison is not case-sensitive, i.e. "München" equals "münchen".
        /// </remarks>
        public bool Equals(Address other)
        {
            return AddressLine.Equals(other.AddressLine, StringComparison.OrdinalIgnoreCase) &&
                   AddressLine2.Equals(other.AddressLine2, StringComparison.OrdinalIgnoreCase) &&
                   City.Equals(other.City, StringComparison.OrdinalIgnoreCase) &&
                   PostalCode.Equals(other.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                   StateOrProvince.Equals(other.StateOrProvince, StringComparison.OrdinalIgnoreCase) &&
                   Country.Equals(other.Country, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Address other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                AddressLine.ToUpperInvariant(),
                AddressLine2.ToUpperInvariant(),
                City.ToUpperInvariant(),
                PostalCode.ToUpperInvariant(),
                StateOrProvince.ToUpperInvariant(),
                Country.ToUpperInvariant()
            );
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{AddressLine}, {AddressLine2}, {City}, {PostalCode}, {StateOrProvince}, {Country}";
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

    /// <summary>
    /// Compares two instances of <see cref="Address"/> for equality.
    /// We provide this comparer to allow for case-sensitive comparisons, as the default comparer is not case-sensitive.
    /// </summary>
    /// <remarks>
    /// The comparison is case-sensitive, i.e. "München" does not equal "münchen".
    /// </remarks>
    public class CaseSensitiveAddressComparer : IEqualityComparer<Address>
    {
        /// <inheritdoc/>
        public bool Equals(Address x, Address y)
        {
            return x.AddressLine.Equals(y.AddressLine) &&
                   x.AddressLine2.Equals(y.AddressLine2) &&
                   x.City.Equals(y.City) &&
                   x.PostalCode.Equals(y.PostalCode) &&
                   x.StateOrProvince.Equals(y.StateOrProvince) &&
                   x.Country.Equals(y.Country);
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Address obj)
        {
            return HashCode.Combine(obj.AddressLine, obj.AddressLine2, obj.City, obj.PostalCode, obj.StateOrProvince, obj.Country);
        }
    }
}
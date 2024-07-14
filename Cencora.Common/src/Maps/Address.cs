// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json.Serialization;

namespace Cencora.Common.Maps;

/// <summary>
/// Represents an address.
/// </summary>
public struct Address : IEquatable<Address>
{
    /// <summary>
    /// Gets an empty address.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static Address Empty => new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> struct.
    /// </summary>
    public Address()
    {}

    /// <summary>
    /// Gets or sets the first line of the address.
    /// </summary>
    [JsonInclude]
    public string AddressLine1 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the second line of the address.
    /// </summary>
    [JsonInclude]
    public string AddressLine2 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city of the address.
    /// </summary>
    [JsonInclude]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the postal code of the address.
    /// </summary>
    [JsonInclude]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state or province of the address.
    /// </summary>
    [JsonInclude]
    public string StateOrProvince { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country of the address.
    /// </summary>
    [JsonInclude]
    public string Country { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsEmpty => Equals(Empty);

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

    public static bool operator ==(Address left, Address right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Address left, Address right)
    {
        return !left.Equals(right);
    }
}
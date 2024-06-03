// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Core;

namespace Cencora.Common.Maps
{
    /// <summary>
    /// Provides extension methods for the <see cref="Address"/> class.
    /// </summary>
    public static partial class AddressExtensions
    {
        /// <summary>
        /// Returns a string that represents the address in a format that can be used in a query string for a map service.
        /// </summary>
        /// <returns>A string that represents the address in a format that can be used in a query string for a map service.</returns>
        public static string MapsQueryString(this Address address)
        {
            // For whatever reason, the postal code in the query string should not contain the dash, if searched for in Azure Maps.
            // We remove the dash here. I do not know if this is also the case for other map services.
            var parts = new[] 
            { 
                address.AddressLine, 
                address.AddressLine2, 
                address.City, 
                address.PostalCode.Replace("-", string.Empty), 
                address.StateOrProvince, 
                address.Country 
            };
            return string.Join(", ", parts.Where(p => !string.IsNullOrEmpty(p)));
        }
    }
}
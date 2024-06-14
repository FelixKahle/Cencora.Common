// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents an address.
    /// </summary>
    public struct Address
    {
        private string _addressLine1 = string.Empty;
        private string _addressLine2 = string.Empty;
        private string _city = string.Empty;
        private string _postalCode = string.Empty;
        private string _stateOrProvince = string.Empty;
        private string _country = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        public Address()
        {}

        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        public string AddressLine1
        {
            get => _addressLine1;
            set => _addressLine1 = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        public string AddressLine2
        {
            get => _addressLine2;
            set => _addressLine2 = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        public string City
        {
            get => _city;
            set => _city = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        public string PostalCode
        {
            get => _postalCode;
            set => _postalCode = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the state or province of the address.
        /// </summary>
        public string StateOrProvince
        {
            get => _stateOrProvince;
            set => _stateOrProvince = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        public string Country
        {
            get => _country;
            set => _country = value ?? string.Empty;
        }
    }
}
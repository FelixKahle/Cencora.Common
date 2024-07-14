// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Globalization;
using System.Text.Json.Serialization;
using Cencora.Common.Measurements;

namespace Cencora.Common.Maps
{
    /// <summary>
    /// Represents a geographic coordinate.
    /// </summary>
    /// <remarks>
    /// The latitude and longitude are stored in degrees.
    /// If you try to set a latitude or longitude that is out of range, an <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </remarks>
    public struct GeoCoordinate : IEquatable<GeoCoordinate>
    {
        private double _latitude;
        private double _longitude;

        public static readonly GeoCoordinate Zero = new GeoCoordinate(0, 0);
        public static readonly GeoCoordinate Unknown = new GeoCoordinate(double.NaN, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate"/> class.
        /// Both latitude and longitude are set to 0.
        /// </summary>
        public GeoCoordinate()
        {
            _latitude = double.NaN;
            _longitude = double.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <exception cref="ArgumentOutOfRangeException">When latitude or longitude are out of range</exception>
        [JsonConstructor]
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        /// <exception cref="ArgumentOutOfRangeException">When latitude is out of range</exception>
        [JsonInclude]
        public double Latitude
        {
            get => _latitude;
            set
            {
                if (value > 90.0 || value < -90.0)
                {
                    throw new ArgumentOutOfRangeException($"Latitude must be between -90 and 90 degrees.");
                }
                _latitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        /// <exception cref="ArgumentOutOfRangeException">When longitude is out of range</exception>
        [JsonInclude]
        public double Longitude
        {
            get => _longitude;
            set
            {
                if (value > 180.0 || value < -180.0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(Longitude)} must be between -180 and 180 degrees.");
                }
                _longitude = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance represents an unknown coordinate.
        /// </summary>
        [JsonInclude]
        public bool IsUnknown => Equals(Unknown);

        /// <inheritdoc/>
        public bool Equals(GeoCoordinate other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is GeoCoordinate other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        /// <summary>
        /// Returns the distance between two coordinates.
        /// </summary>
        /// <param name="other">The other coordinate.</param>
        /// <returns>The distance between the two coordinates. </returns>
        /// <exception cref="ArgumentException">When latitude or longitude are not a number</exception>
        public Distance GetDistanceTo(GeoCoordinate other)
        {
            //  The Haversine formula according to Dr. Math.
            //  http://mathforum.org/library/drmath/view/51879.html

            if (double.IsNaN(Latitude) || double.IsNaN(Longitude) ||
                double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number.");
            }

            double lat1 = Latitude * (Math.PI / 180.0);
            double lon1 = Longitude * (Math.PI / 180.0);
            double lat2 = other.Latitude * (Math.PI / 180.0);
            double lon2 = other.Longitude * (Math.PI / 180.0);

            double lon = lon2 - lon1;
            double lat = lat2 - lat1;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(lat / 2.0), 2.0) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(lon / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            const double kEarthRadiusMs = 6376500;
            double distance = kEarthRadiusMs * c;
            return new Distance(distance, DistanceUnit.Meter);
        }

        /// <summary>
        /// Returns a string that represents the current GeoCoordinate.
        /// For debugging purposes only.
        /// </summary>
        /// <returns>A string that represents the current GeoCoordinate.</returns>
        public override string ToString()
        {
            return $"Latitude: {Latitude.ToString(CultureInfo.InvariantCulture)}, Longitude: {Longitude.ToString(CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Compares two GeoCoordinate instances for equality.
        /// </summary>
        /// <param name="left">The first GeoCoordinate instance.</param>
        /// <param name="right">The second GeoCoordinate instance.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two GeoCoordinate instances for inequality.
        /// </summary>
        /// <param name="left">The first GeoCoordinate instance.</param>
        /// <param name="right">The second GeoCoordinate instance.</param>
        /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !left.Equals(right);
        }
    }
}

// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Maps;

namespace Cencora.Common.Tests.Maps;

public class GeoCoordinateTests
{
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        var geoCoordinate = new GeoCoordinate();
        Assert.Equal(double.NaN, geoCoordinate.Latitude);
        Assert.Equal(double.NaN, geoCoordinate.Longitude);
        Assert.Equal(GeoCoordinate.Unknown, geoCoordinate);
    }

    [Fact]
    public void IsUnknown_WithUnknownCoordinate_ShouldReturnTrue()
    {
        var geoCoordinate = new GeoCoordinate();
        Assert.True(geoCoordinate.IsUnknown);
    }

    [Fact]
    public void Constructor_WithParameters_ShouldCreateInstance()
    {
        var geoCoordinate = new GeoCoordinate(1, 2);
        Assert.Equal(1, geoCoordinate.Latitude);
        Assert.Equal(2, geoCoordinate.Longitude);
    }

    [Fact]
    public void Latitude_ShouldSetLatitude()
    {
        var geoCoordinate = new GeoCoordinate
        {
            Latitude = 1
        };
        Assert.Equal(1, geoCoordinate.Latitude);
    }

    [Fact]
    public void Longitude_ShouldSetLongitude()
    {
        var geoCoordinate = new GeoCoordinate
        {
            Longitude = 1
        };
        Assert.Equal(1, geoCoordinate.Longitude);
    }

    [Fact]
    public void Latitude_WithOutOfRangeValue_ShouldThrowException()
    {
        var geoCoordinate = new GeoCoordinate();
        Assert.Throws<ArgumentOutOfRangeException>(() => geoCoordinate.Latitude = 91);
    }

    [Fact]
    public void Longitude_WithOutOfRangeValue_ShouldThrowException()
    {
        var geoCoordinate = new GeoCoordinate();
        Assert.Throws<ArgumentOutOfRangeException>(() => geoCoordinate.Longitude = 181);
    }

    [Fact]
    public void Equals_WithEqualCoordinates_ShouldReturnTrue()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(1, 2);
        Assert.True(geoCoordinate1.Equals(geoCoordinate2));
        Assert.True(geoCoordinate1 == geoCoordinate2);
        Assert.False(geoCoordinate1 != geoCoordinate2);
    }

    [Fact]
    public void Equals_WithDifferentCoordinates_ShouldReturnFalse()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(2, 1);
        Assert.False(geoCoordinate1.Equals(geoCoordinate2));
        Assert.False(geoCoordinate1 == geoCoordinate2);
        Assert.True(geoCoordinate1 != geoCoordinate2);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        var geoCoordinate = new GeoCoordinate(1, 2);
        Assert.False(geoCoordinate.Equals(null));
    }

    [Fact]
    public void GetHashCode_WithEqualCoordinates_ShouldReturnEqualHashCodes()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(1, 2);
        Assert.Equal(geoCoordinate1.GetHashCode(), geoCoordinate2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentCoordinates_ShouldReturnDifferentHashCodes()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(2, 1);
        Assert.NotEqual(geoCoordinate1.GetHashCode(), geoCoordinate2.GetHashCode());
    }

    [Fact]
    public void GetDistanceTo_WithSameCoordinates_ShouldReturnZero()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(1, 2);
        var distance = geoCoordinate1.GetDistanceTo(geoCoordinate2);

        Assert.Equal(0, distance.Meters);
    }

    [Fact]
    public void GetDistanceTo_WithDifferentCoordinates_ShouldReturnDistance()
    {
        var geoCoordinate1 = new GeoCoordinate(1, 2);
        var geoCoordinate2 = new GeoCoordinate(5, 6);
        var distance = geoCoordinate1.GetDistanceTo(geoCoordinate2);

        Assert.Equal(629, distance.Kilometers, 0.1);
    }
}
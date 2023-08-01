using System;
using AspNetCoreSample.Foundation.WeatherApi.DataContracts;

namespace AspNetCoreSample.Models;

public sealed class Location
{
    public string Name { get; set; }

    public string Region { get; set; }

    public string Country { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime LocalTime { get; set; }


    private Location(string name, string region, string country, double latitude, double longitude, DateTime localTime)
    {
        Name = name;
        Region = region;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        LocalTime = localTime;
    }


    public static Location CreateFrom(LocationDataContract location) 
        => new(location.Name, location.Region, location.Country, location.Lat, location.Lon, location.LocalTime);
}
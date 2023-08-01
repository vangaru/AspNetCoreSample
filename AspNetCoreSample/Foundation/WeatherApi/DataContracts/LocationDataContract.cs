using System;

namespace AspNetCoreSample.Foundation.WeatherApi.DataContracts;

public sealed class LocationDataContract
{
    public string Name { get; set; }

    public string Region { get; set; }

    public string Country { get; set; }

    public double Lat { get; set; }

    public double Lon { get; set; }

    public DateTime LocalTime { get; set; }
}
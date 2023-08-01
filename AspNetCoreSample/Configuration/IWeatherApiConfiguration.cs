using System;

namespace AspNetCoreSample.Configuration;

public interface IWeatherApiConfiguration
{
    Uri ApiUrl { get; }

    string ApiKey { get; }
}
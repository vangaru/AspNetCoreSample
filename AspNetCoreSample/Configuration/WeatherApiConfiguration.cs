using System;
using AspNetCoreSample.Options;
using Microsoft.Extensions.Options;

namespace AspNetCoreSample.Configuration;

public sealed class WeatherApiConfiguration : IWeatherApiConfiguration
{
    private readonly WeatherApiOptions _options;


    public Uri ApiUrl => new(_options.ApiUrl);

    public string ApiKey => _options.ApiKey;


    public WeatherApiConfiguration(IOptions<WeatherApiOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;
    }
}
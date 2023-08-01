namespace AspNetCoreSample.Options;

public sealed class WeatherApiOptions
{
    public const string SectionName = "WeatherApi";


    public string ApiUrl { get; set; }

    public string ApiKey { get; set; }
}
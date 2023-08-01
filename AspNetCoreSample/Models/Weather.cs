using AspNetCoreSample.Foundation.WeatherApi.DataContracts;

namespace AspNetCoreSample.Models;

public sealed class Weather
{
    public double TemperatureCelsius { get; set; }

    public double TemperatureFahrenheit { get; set; }

    public bool IsDay { get; set; }

    public double WindMph { get; set; }

    public double WindKph { get; set; }

    public double WindDegree { get; set; }


    private Weather(double temperatureCelsius, double temperatureFahrenheit, bool isDay, double windMph, double windKph, double windDegree)
    {
        TemperatureCelsius = temperatureCelsius;
        TemperatureFahrenheit = temperatureFahrenheit;
        IsDay = isDay;
        WindMph = windMph;
        WindKph = windKph;
        WindDegree = windDegree;
    }


    public static Weather CreateFrom(WeatherDataContract weather)
        => new(weather.TempC, weather.TempF, weather.IsDay, weather.WindMph, weather.WindKph, weather.WindDegree);
}
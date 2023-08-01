using AspNetCoreSample.Foundation.WeatherApi.DataContracts;

namespace AspNetCoreSample.Models;

public sealed class WeatherProfile
{
    public Location Location { get; set; }

    public Weather CurrentWeather { get; set; }


    private WeatherProfile(Location location, Weather currentWeather)
    {
        Location = location;
        CurrentWeather = currentWeather;
    }


    public static WeatherProfile CreateFrom(WeatherProfileDataContract weatherProfile)
        => new(Location.CreateFrom(weatherProfile.Location), Weather.CreateFrom(weatherProfile.Current));
}
namespace AspNetCoreSample.Foundation.WeatherApi.DataContracts;

public class WeatherProfileDataContract
{
    public LocationDataContract Location { get; set; }

    public WeatherDataContract Current { get; set; }
}
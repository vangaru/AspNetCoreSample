namespace AspNetCoreSample.Foundation.WeatherApi.DataContracts;

public sealed class WeatherDataContract
{
    public double TempC { get; set; }

    public double TempF { get; set; }

    public bool IsDay { get; set; }

    public double WindMph { get; set; }

    public double WindKph { get; set; }

    public double WindDegree { get; set; }
}
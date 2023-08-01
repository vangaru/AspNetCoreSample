using System.Threading.Tasks;
using AspNetCoreSample.Common;
using AspNetCoreSample.Foundation.WeatherApi.DataContracts;

namespace AspNetCoreSample.Foundation.WeatherApi;

public interface IWeatherApiClient
{
    Task<OperationResult<WeatherProfileDataContract>> GetWeatherProfileAsync(string query);
}
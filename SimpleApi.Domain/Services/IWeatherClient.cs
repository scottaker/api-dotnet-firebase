using SimpleApi.Domain.Models;

namespace SimpleApi.Domain.Services;

public interface IWeatherClient
{
    Task<WeatherData?> Get(double latitude, double longitude);

    Task<IEnumerable<WeatherData>> Get(IEnumerable<Tuple<double, double>> locations);
}
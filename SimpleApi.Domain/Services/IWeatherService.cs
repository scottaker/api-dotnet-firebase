using SimpleApi.Domain.Models;

namespace SimpleApi.Domain.Services;

public interface IWeatherService
{
    Task<WeatherData?> Get(double latitude, double longitude);
    Task<IEnumerable<WeatherData>> Get(IEnumerable<Tuple<double, double>> locations);
    Task<WeatherData> GetAtAddress(string address);
}
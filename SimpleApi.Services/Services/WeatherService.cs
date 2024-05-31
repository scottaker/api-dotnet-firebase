using SimpleApi.Domain.Models;
using SimpleApi.Domain.Services;

namespace SimpleApi.Services.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherClient _client;
    private readonly ILocationService _location;

    public WeatherService(IWeatherClient client, ILocationService location)
    {
        _client = client;
        _location = location;
    }

    public async Task<WeatherData> Get(double latitude, double longitude)
    {
        var data = await _client.Get(latitude, longitude);
        return data;
    }
    public async Task<IEnumerable<WeatherData>> Get(IEnumerable<Tuple<double, double>> locations)
    {
        var data = await _client.Get(locations);
        return data;
    }

    public async Task<WeatherData> GetAtAddress(string address)
    {
        var locations = await _location.Find(address);
        if (locations == null || !locations.HasData())
        {
            return null;
        }

        var location = locations.Results.FirstOrDefault();
        if (location == null || location.Location == null) return null;

        var gps = location.Location;
        var weather = await _client.Get(gps.Latitude, gps.Longitude);
        return weather;
    }

}
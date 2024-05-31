using SimpleApi.Domain.Services;
using System.Text.Json;
using SimpleApi.Domain.Models;

namespace SimpleApi.Services.Clients;

public class WeatherClient : IWeatherClient
{
    private readonly IClientBase _client;

    public WeatherClient(IClientBase client)
    {
        _client = client;
    }

    public async Task<WeatherData> Get(double latitude, double longitude)
    {
        var uri = $"https://weatherapi-com.p.rapidapi.com/current.json?q={latitude}%2C{longitude}";
        var data = await _client.Get(uri);

        if (data == null) return null;

        var envelope = JsonSerializer.Deserialize<WeatherEnvelope>(data);
        if (envelope == null) return null;

        var weather = envelope.Current;
        weather.Location = envelope.Location;

        return weather ?? null;
    }

    public async Task<IEnumerable<WeatherData>> Get(IEnumerable<Tuple<double, double>> locations)
    {
        var uri = "https://weatherapi-com.p.rapidapi.com/current.json?q={0}%2C{1}";
        var uris = locations.Select(x => string.Format(uri, x.Item1, x.Item2));

        var data = await _client.Get(uris);
        if (data == null) return null;

        var results = data.Select(x => JsonSerializer.Deserialize<WeatherData>(x));
        return results;
    }

}
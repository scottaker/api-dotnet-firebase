using System.Net;
using System.Text.Json;
using SimpleApi.Services.Models;
using SimpleApi.Domain.Services;

namespace SimpleApi.Services.Clients;


public class LocationClient : ILocationClient
{
    private readonly IClientBase _client;

    public LocationClient(IClientBase client)
    {
        _client = client;
    }

    public async Task<AddressResults> Get(string location, string language = "en", string country = "us")
    {
        var query = WebUtility.UrlEncode(location);
        var uri = $"https://trueway-geocoding.p.rapidapi.com/Geocode?address={query}&language={language}";
        var data = await _client.Get(uri);
        if (data == null) return null;

        var results = JsonSerializer.Deserialize<AddressResults>(data);
        return results;
    }

}
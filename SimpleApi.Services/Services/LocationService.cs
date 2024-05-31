using SimpleApi.Domain.Services;
using SimpleApi.Services.Models;

namespace SimpleApi.Services.Services;


public class LocationService : ILocationService
{
    private readonly ILocationClient _client;

    public LocationService(ILocationClient client)
    {
        _client = client;
    }

    public async Task<AddressResults> Find(string location)
    {
        var data = await _client.Get(location);
        return data;
    }

}
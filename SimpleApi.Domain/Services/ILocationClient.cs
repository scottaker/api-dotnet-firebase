using SimpleApi.Services.Models;

namespace SimpleApi.Domain.Services;


public interface ILocationClient
{
    Task<AddressResults> Get(string location, string language = "en", string country = "us");
}
using SimpleApi.Services.Models;

namespace SimpleApi.Domain.Services;

public interface ILocationService
{
    Task<AddressResults> Find(string location);
    
}
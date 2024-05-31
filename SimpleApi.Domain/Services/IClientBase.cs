namespace SimpleApi.Domain.Services;

public interface IClientBase
{
    Task<string> Get(string uri);
    Task<string> Post<T>(string uri, T payload);
    Task<IEnumerable<string>> Get(IEnumerable<string> uris);
}


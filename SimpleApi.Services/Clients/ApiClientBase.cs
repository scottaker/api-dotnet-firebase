using SimpleApi.Domain.Services;

namespace SimpleApi.Services.Services;


public abstract class ApiClientBase : IClientBase
{
    protected readonly IHttpClientFactory _factory;
    private readonly ILogger _logger;
    public bool AcceptGzipEncoding => true;

    protected ApiClientBase(IHttpClientFactory factory, ILogger logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public abstract Task<string> Get(string uri);
    public abstract Task<IEnumerable<string>> Get(IEnumerable<string> uris);
    public abstract Task<string> Post<T>(string uri, T payload);
}
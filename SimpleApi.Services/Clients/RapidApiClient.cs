using System.Text.Json;
using SimpleApi.Domain.Services;

namespace SimpleApi.Services.Services;

public class RapidApiClient : ApiClientBase
{
    private readonly ApiSettings _settings;
    public bool AcceptGzipEncoding => true;

    public RapidApiClient(IHttpClientFactory factory, ILogger logger, ApiSettings settings) : base(factory, logger)
    {
        _settings = settings;
    }

    public override async Task<string> Get(string uri)
    {
        var request = CreateRequest(uri, HttpMethod.Get);
        var client = _factory.CreateClient("rapid-api");

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }
    }
    public override async Task<IEnumerable<string>> Get(IEnumerable<string> uris)
    {

        var tasks = new List<Task<string>>();

        foreach (var uri in uris)
        {
            var task = Get(uri);
            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);
        return results;
    }

    public override async Task<string> Post<T>(string uri, T payload)
    {
        var request = CreateRequest(uri, HttpMethod.Post);

        var jsonPayload = JsonSerializer.Serialize(payload);
        request.Content = new StringContent(jsonPayload);

        var client = _factory.CreateClient("rapid-api");
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }
    }

    private HttpRequestMessage CreateRequest(string uri, HttpMethod method)
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(uri),
            Headers =
            {
                { "X-RapidAPI-Key", _settings.Key },
                { "X-RapidAPI-Host", _settings.Host },
            },
        };
        return request;
    }
}
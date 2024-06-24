using AutoMapper;
using FirebaseDotnet.Data;
using SimpleApi.API.Services;
using SimpleApi.Domain.Services;
using SimpleApi.Services.Clients;
using SimpleApi.Services.Services;
using ILogger = SimpleApi.Domain.Services.ILogger;

namespace SimpleApi.API.Infrastructure;


public class DependencyManager
{
    public void Services(IServiceCollection services, IConfiguration configuration)
    {
        IServiceCollection Add<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            return services.AddScoped<TInterface, TClass>();
        }

        // application infrastructure
        services.AddHttpClient();
        Add<ILogger, DefaultLogger>();
        services.AddSingleton<SimpleApiSettings>(provider => GetConfig<SimpleApiSettings>(configuration, "SimpleApiSettings"));
        services.AddSingleton<ApiClientFactory>();

        // API CLIENTS
        services.AddScoped<ILocationClient>((p) =>
        {
            var clientFactory = p.GetService<ApiClientFactory>();
            var httpFactory = p.GetService<IHttpClientFactory>();
            var settings = p.GetService<SimpleApiSettings>();
            var logger = p.GetService<ILogger>();

            var client = clientFactory.Create(httpFactory, logger, settings.LocationApiSettings);
            return new LocationClient(client);
        });
        services.AddScoped<IWeatherClient>((p) =>
        {
            var clientFactory = p.GetService<ApiClientFactory>();
            var httpFactory = p.GetService<IHttpClientFactory>();
            var settings = p.GetService<SimpleApiSettings>();
            var logger = p.GetService<ILogger>();

            var client = clientFactory.Create(httpFactory, logger, settings.WeatherApiSettings);
            return new WeatherClient(client);
        });

        // handle mapping dependencies
        ConfigureMapper(services);
        services.AddSingleton<SimpleApi.Domain.Services.IMapper>((p) =>
        {
            var mapper = p.GetService<AutoMapper.IMapper>();
            return new DefaultMapper(mapper);
        });


        // SERVICES
        Add<ILocationService, LocationService>();
        Add<IWeatherService, WeatherService>();

        // Clients - Firestore
        services.AddScoped<FirestoreClient>();
        Add<ISupportDataClient, SupportDataClient>();

        Add<IComplaintClient, ComplaintClient>();
        Add<IComplaintService, ComplaintService>();
        Add<IBranchClient, BranchClient>();


        services.AddScoped<CityService>();

        //Add<IGPTCLient, GptClient>();
        //Add<IGptService, GptService>();
    }

    private static void ConfigureMapper(IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    private static T GetConfig<T>(IConfiguration configuration, string name)
    {
        var config = configuration.GetSection(name).Get<T>();
        return config;
    }

    public class ApiClientFactory
    {
        public RapidApiClient Create(IHttpClientFactory factory, ILogger logger, ApiSettings settings)
        {
            return new RapidApiClient(factory, logger, settings);
        }
    }
}
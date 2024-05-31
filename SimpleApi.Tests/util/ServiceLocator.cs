using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleApi.API.Infrastructure;

namespace SimpleApi.Tests.util;

public class ServiceLocator
{
    private readonly ServiceProvider _container;

    public ServiceLocator(Action<IServiceCollection>? mocks = null)
    {
        var services = new ServiceCollection();
        //var settings = GetConfiguration("Development");

        var manager = new DependencyManager();
        var config = InitConfiguration();
        manager.Services(services, config);

        // clients can send in mocks for tests
        if (mocks != null)
        {
            mocks(services);
        }

        _container = services.BuildServiceProvider();
    }


    public T Get<T>() where T : class
    {
        var item = _container.GetService<T>();
        return item;
    }

    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json")
                     .Build();
        return config;
    }

}


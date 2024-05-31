using NUnit.Framework;
using NUnit.Framework.Legacy;
using SimpleApi.Domain.Models;
using SimpleApi.Domain.Services;
using SimpleApi.Services.Models;
using SimpleApi.Tests.util;

namespace SimpleApi.Tests;

[TestFixture]
public class LocationTests
{
    [Test]
    public async Task Geocode()
    {
        var locator = GetLocator();
        var locationService = locator.Get<ILocationService>();

        // find location
        var location = await locationService.Find("505 Howard St, San Francisco");
        ClassicAssert.IsNotNull(location);

        Show(location);
    }

    [Test]
    public async Task Geocode_Bad_Data()
    {
        var locator = GetLocator();
        var locationService = locator.Get<ILocationService>();

        // find location
        var location = await locationService.Find("505 blah");
        ClassicAssert.IsNotNull(location);
        ClassicAssert.IsEmpty(location.Results);
        ClassicAssert.IsFalse(location.HasData());
        
        Show(location);
    }

    [Test]
    public async Task Get_City_Weather()
    {
        var locator = GetLocator();
        var locationService = locator.Get<ILocationService>();
        var weatherService = locator.Get<IWeatherService>();

        // find location
        var address = await locationService.Find("505 Howard St, San Francisco");
        ClassicAssert.IsNotNull(address);

        // find weather
        var location = address.Results.First().Location;
        var weather = await weatherService.Get(location.Latitude, location.Longitude);
        ClassicAssert.IsNotNull(address);

        Console.WriteLine("-- LOCATION --");
        Show(address);
        Console.WriteLine("-- WEATHER --");
        Show(weather);
    }

    private void Show(WeatherData weather)
    {
        Console.WriteLine($"{weather.Location.Name,-25} {weather.TempF,-5}  {weather.PrecipMm} mm");
    }

    private void Show(AddressResults results)
    {

        foreach (var location in results.Results)
        {
            Console.WriteLine($"{location.Address,-45} {location.Location.Latitude,-7} {location.Location.Longitude,-7}");
        }

    }


    private ServiceLocator GetLocator()
    {
        return new ServiceLocator(null);
    }

}
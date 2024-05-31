using NUnit.Framework;
using NUnit.Framework.Legacy;
using SimpleApi.API.Services;
using SimpleApi.Domain.Models;
using SimpleApi.Domain.Services;
using SimpleApi.Tests.util;

namespace SimpleApi.Tests;

[TestFixture]
public class WeatherTests
{

    [Test]
    public async Task Get_City_Weather()
    {
        var locator = GetLocator();
        var cityService = locator.Get<CityService>();
        var weatherService = locator.Get<IWeatherService>();


        var cities = cityService.GetCities();
        var city = cities.First();

        var weather = await weatherService.Get(city.GpsCoordinates.Latitude, city.GpsCoordinates.Longitude);

        ClassicAssert.IsNotNull(weather);

        Show(weather);
    }


    [Test]
    public async Task Address_Weather()
    {
        var locator = GetLocator();
        var weatherService = locator.Get<IWeatherService>();

        var address = "505 Howard St, San Francisco";
        var weather = await weatherService.GetAtAddress(address);

        ClassicAssert.IsNotNull(weather);

        Show(weather);
    }

    [Test]
    public async Task Bad_Address_Is_Empty()
    {
        var locator = GetLocator();
        var weatherService = locator.Get<IWeatherService>();

        var address = "505 blah";
        var weather = await weatherService.GetAtAddress(address);

        ClassicAssert.IsNull(weather);

    }

    //ClassicAssert.IsNotNull(location);
    //ClassicAssert.IsEmpty(location.Results);
    //ClassicAssert.IsFalse(location.HasData());

    private void Show(WeatherData weather)
    {
        Console.WriteLine($"{weather.Location.Name,-25} {weather.TempF,-5} {weather.PrecipMm}");
    }


    private ServiceLocator GetLocator()
    {
        return new ServiceLocator(null);
    }

}
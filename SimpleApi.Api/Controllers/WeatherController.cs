using Microsoft.AspNetCore.Mvc;
using SimpleApi.API.Services;
using SimpleApi.Domain.Models;
using SimpleApi.Domain.Services;

namespace SimpleApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly CityService _cityService;

    public WeatherController(IWeatherService weatherService, CityService cityService)
    {
        _weatherService = weatherService;
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<WeatherData> GetWeather()
    {
        var cities = _cityService.GetCities();
        var city = cities.First();

        var weather = await _weatherService.Get(city.GpsCoordinates.Latitude, city.GpsCoordinates.Longitude);

        return weather;
    }


    [HttpPost("location")]
    public async Task<WeatherData> GetWeather([FromBody] GpsRequest request)
    {
        var weather = await _weatherService.Get(request.Latitude, request.Longitude);
        return weather;
    }


    [HttpPost("address")]
    public async Task<WeatherData> GetWeather([FromBody] AddressRequest request)
    {
        var weather = await _weatherService.GetAtAddress(request.Address);
        return weather;
    }


}


public class GpsRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class AddressRequest
{
    public string Address { get; set; }
}
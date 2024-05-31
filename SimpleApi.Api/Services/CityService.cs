using System.Text.Json;
using SimpleApi.Services.Models;

namespace SimpleApi.API.Services
{
    public class CityService
    {

        public CityService() { }

        public IEnumerable<City> GetCities()
        {
            // Define the relative path to the JSON file
            var filepath = Path.Combine("Data", "city_data.json");
            var absolutePath = Path.GetFullPath(filepath);
            var jsonString = File.ReadAllText(absolutePath);


            var cities = JsonSerializer.Deserialize<IEnumerable<City>>(jsonString);
            return cities;
        }
    }
}

using NUnit.Framework;
using NUnit.Framework.Legacy;
using SimpleApi.API.Services;
using SimpleApi.Services.Models;

namespace SimpleApi.Tests
{
    [TestFixture]
    public class BasicTests
    {

        [Test]
        public void Get_Cities()
        {
            var service = new CityService();
            var cities = service.GetCities();

            ClassicAssert.IsNotNull(cities);
            ClassicAssert.IsNotEmpty(cities);

            Show(cities);
        }



        private void Show(IEnumerable<City> cities)
        {
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.CityName,-30}\t{city.GpsCoordinates.Latitude} {city.GpsCoordinates.Longitude}");
            }
        }
    }
}

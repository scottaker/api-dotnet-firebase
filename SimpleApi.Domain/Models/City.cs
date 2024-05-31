using System.Text.Json.Serialization;

namespace SimpleApi.Services.Models;

public class City
{
    [JsonPropertyName("city_name")]
    public string CityName { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("gps_coordinates")]
    public GpsCoordinates GpsCoordinates { get; set; }

    [JsonPropertyName("population")]
    public int Population { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }
}

public class GpsCoordinates
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}
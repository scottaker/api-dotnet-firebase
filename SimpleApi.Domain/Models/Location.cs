using System.Text.Json.Serialization;

namespace SimpleApi.Services.Models;

public class AddressResults
{
    [JsonPropertyName("results")]
    public List<AddressResult> Results { get; set; }

    public bool HasData()
    {
        return this.Results != null && this.Results.Count > 0;
    }
}

public class AddressResult
{
    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("postal_code")]
    public string PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("area")]
    public string Area { get; set; }

    [JsonPropertyName("locality")]
    public string Locality { get; set; }

    [JsonPropertyName("neighborhood")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("house")]
    public string House { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("location_type")]
    public string LocationType { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Location
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

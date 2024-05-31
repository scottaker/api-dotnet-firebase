using System.Text.Json.Serialization;

namespace SimpleApi.Domain.Models;

public class WeatherEnvelope
{
    [JsonPropertyName("current")]
    public WeatherData Current { get; set; }
    [JsonPropertyName("location")]
    public WeatherLocation Location { get; set; }
}

public class WeatherData
{
    [JsonPropertyName("condition")]
    public Condition Condition { get; set; }

    [JsonPropertyName("location")]
    public WeatherLocation Location { get; set; }

    [JsonPropertyName("cloud")]
    public int Cloud { get; set; }

    [JsonPropertyName("dewpoint_c")]
    public double DewpointC { get; set; }

    [JsonPropertyName("dewpoint_f")]
    public double DewpointF { get; set; }

    [JsonPropertyName("feelslike_c")]
    public double FeelslikeC { get; set; }

    [JsonPropertyName("feelslike_f")]
    public double FeelslikeF { get; set; }

    [JsonPropertyName("gust_kph")]
    public double GustKph { get; set; }

    [JsonPropertyName("gust_mph")]
    public double GustMph { get; set; }

    [JsonPropertyName("heatindex_c")]
    public double HeatindexC { get; set; }

    [JsonPropertyName("heatindex_f")]
    public double HeatindexF { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("is_day")]
    public int IsDay { get; set; }

    [JsonPropertyName("last_updated")]
    public string LastUpdated { get; set; }

    [JsonPropertyName("last_updated_epoch")]
    public long LastUpdatedEpoch { get; set; }

    [JsonPropertyName("precip_in")]
    public double PrecipIn { get; set; }

    [JsonPropertyName("precip_mm")]
    public double PrecipMm { get; set; }

    [JsonPropertyName("pressure_in")]
    public double PressureIn { get; set; }

    [JsonPropertyName("pressure_mb")]
    public double PressureMb { get; set; }

    [JsonPropertyName("temp_c")]
    public double TempC { get; set; }

    [JsonPropertyName("temp_f")]
    public double TempF { get; set; }

    [JsonPropertyName("uv")]
    public double UV { get; set; }

    [JsonPropertyName("vis_km")]
    public double VisKm { get; set; }

    [JsonPropertyName("vis_miles")]
    public double VisMiles { get; set; }

    [JsonPropertyName("wind_degree")]
    public int WindDegree { get; set; }

    [JsonPropertyName("wind_dir")]
    public string WindDir { get; set; }

    [JsonPropertyName("wind_kph")]
    public double WindKph { get; set; }

    [JsonPropertyName("wind_mph")]
    public double WindMph { get; set; }


    [JsonPropertyName("windchill_c")]
    public double WindchillC { get; set; }

    [JsonPropertyName("windchill_f")]
    public double WindchillF { get; set; }
}

public class Condition
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class WeatherLocation
{
    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("localtime")]
    public string LocalTime { get; set; }

    [JsonPropertyName("localtime_epoch")]
    public long LocalTimeEpoch { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("tz_id")]
    public string TimeZoneId { get; set; }
}

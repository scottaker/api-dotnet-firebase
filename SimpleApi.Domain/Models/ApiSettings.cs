
public class SimpleApiSettings
{
    public ApiSettings WeatherApiSettings { get; set; }
    public ApiSettings LocationApiSettings { get; set; }
}

public class ApiSettings
{
    public string Key { get; set; }
    public string Host { get; set; }
}
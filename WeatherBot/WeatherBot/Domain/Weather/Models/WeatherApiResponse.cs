namespace WeatherBot.Domain.Weather.Models;

public class WeatherApiResponse
{
    public TemperatureInfo Main { set; get; }
    public string Name { get; set; }
}

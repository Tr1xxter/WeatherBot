namespace WeatherBot.Domain.Weather.Models;

public class WeatherApiResponse
{
    public List<WeatherInfo> Weather { get; set; }
    public TemperatureInfo Main { set; get; }
    public string Name { get; set; }

    public WeatherInfo WeatherInfo => Weather[0];
}

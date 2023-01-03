using Newtonsoft.Json;

namespace WeatherBot.Domain.Weather.Models;

public class TemperatureInfo
{
    public float Temp { get; set; }

    [JsonProperty("feels_like", NullValueHandling = NullValueHandling.Ignore)]
    public float? FeelsLike { get; set; }
}

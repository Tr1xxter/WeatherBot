using Newtonsoft.Json;

namespace WeatherBot.Domain.Weather.Models;

public class CityCoordinates
{
    [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
    public float Latitude { get; set; }

    [JsonProperty("lon", NullValueHandling = NullValueHandling.Ignore)]
    public float Longitude { get; set; }

    public string Name { get; set; }
}

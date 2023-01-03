using System.Security.Cryptography;

namespace WeatherBot.Domain.Weather.Models;

public class CityCoordinates
{
    public float Lat { get; set; }
    public float Lon { get; set; }

    public string Name { get; set; }
}

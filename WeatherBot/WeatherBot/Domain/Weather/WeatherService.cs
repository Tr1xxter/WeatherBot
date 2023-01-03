using System.Net;
using System.Runtime.InteropServices.Marshalling;
using Newtonsoft.Json;
using WeatherBot.Configuration;
using WeatherBot.Domain.Weather.Helpers;
using WeatherBot.Domain.Weather.Models;

namespace WeatherBot.Domain.Weather;

public class WeatherService
{
    private readonly SecretsConfig _secretsConfig;

    public WeatherService(SecretsConfig secretsConfig)
    {
        _secretsConfig = secretsConfig;
    }
    
    public WeatherApiResponse GetCityWeather(string cityName = "Yekaterinburg")
    {
        var cityCoordinates = MakeRequest<List<CityCoordinates>>(GetCoordinatesUrl(cityName)).First();
        return MakeRequest<WeatherApiResponse>(GetWeatherUrl(cityCoordinates.Lat, cityCoordinates.Lon));
    }

    private string GetCoordinatesUrl(string cityName, int limit = 1) => $"http://api.openweathermap.org/geo/1.0/direct?q={cityName},{WeatherHelper.CountryCode.Russia}&limit={limit}&appid={_secretsConfig.WeatherApiKey}";

    private string GetWeatherUrl(float lat, float lon) => $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_secretsConfig.WeatherApiKey}&lang=ru&units=metric";

    private T MakeRequest<T>(string url)
    {
        var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
        var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();

        string response;
        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
        }

        var deserializedResponse = JsonConvert.DeserializeObject<T>(response);

        return deserializedResponse;
    }
}

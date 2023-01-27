using Vostok.Logging.Abstractions;
using WeatherBot.Configuration;
using WeatherBot.Domain.Weather.Helpers;
using WeatherBot.Domain.Weather.Models;

namespace WeatherBot.Domain.Weather;

public class WeatherService
{
    private readonly SecretsConfig _secretsConfig;
    private readonly ILog _log;

    public WeatherService(SecretsConfig secretsConfig, ILog log)
    {
        _secretsConfig = secretsConfig;
        _log = log;
    }

    public WeatherApiResponse? GetCityWeather(string cityName = "Yekaterinburg")
    {
        var coordinatesUrl = GetCoordinatesUrl(cityName);
        var coordinatesRequest = WebHelper.MakeRequest<List<CityCoordinates>>(coordinatesUrl);
        if (coordinatesRequest.Count == default)
        {
            _log.Error("Не удалось получить координаты города");
            return null;
        }

        var cityCoordinates = (coordinatesRequest).First();
        var weatherUrl = GetWeatherUrl(cityCoordinates.Latitude, cityCoordinates.Longitude);
        return WebHelper.MakeRequest<WeatherApiResponse>(weatherUrl);
    }

    public WeatherApiResponse GetCityWeatherByCoordinates(float latitude, float longitude)
        => MakeRequest<WeatherApiResponse>(GetWeatherUrl(latitude, longitude));

    private string GetCoordinatesUrl(string cityName, int limit = 1)
        => $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}," +
           $"{WeatherHelper.CountryCode.Russia}&limit={limit}&appid={_secretsConfig.WeatherApiKey}";

    private string GetWeatherUrl(float latitude, float longitude)
        => $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}" +
           $"&appid={_secretsConfig.WeatherApiKey}&lang=ru&units=metric";
}

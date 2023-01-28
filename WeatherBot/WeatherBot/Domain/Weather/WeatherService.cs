using Vostok.Logging.Abstractions;
using WeatherBot.Configuration;
using WeatherBot.Domain.Weather.Helpers;
using WeatherBot.Domain.Weather.Models;
using WeatherBot.Domain.Web.Helpers;

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
        var coordinatesUri = GetCoordinatesUri(cityName);
        var coordinatesRequest = WebHelper.MakeRequest<List<CityCoordinates>>(coordinatesUri, _log);

        if (coordinatesRequest == default || coordinatesRequest.Count == 0)
        {
            _log.Error("Не удалось получить координаты города");
            return null;
        }

        var cityCoordinates = coordinatesRequest.First();
        var weatherUri = GetWeatherUri(cityCoordinates.Latitude, cityCoordinates.Longitude);
        return WebHelper.MakeRequest<WeatherApiResponse>(weatherUri, _log);
    }

    public WeatherApiResponse? GetCityWeatherByCoordinates(float latitude, float longitude)
        => WebHelper.MakeRequest<WeatherApiResponse>(GetWeatherUri(latitude, longitude), _log);

    private string GetCoordinatesUri(string cityName, int limit = 1)
        => $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}," +
           $"{WeatherHelper.CountryCode.Russia}&limit={limit}&appid={_secretsConfig.WeatherApiKey}";

    private string GetWeatherUri(float latitude, float longitude)
        => $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}" +
           $"&appid={_secretsConfig.WeatherApiKey}&lang=ru&units=metric";
}

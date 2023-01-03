using WeatherBot.Domain.Weather.Models;

namespace WeatherBot.Domain.Weather.Helpers;

public static class WeatherHelper
{
    public static class CountryCode
    {
        public static string Russia = "ISO 3166-2:RU";
    }

    public static string GetWeatherApiResponseString(WeatherApiResponse weatherApiResponse)
    {
        var city = weatherApiResponse.Name;
        city = "Екатеринбург";
        return $"Погода в городе {city}: {weatherApiResponse.WeatherInfo.Description}, " +
               $"температура {(int)weatherApiResponse.Main.Temp}, " +
               $"ощущается как {(int)weatherApiResponse.Main.FeelsLike}";
    }
}

using WeatherBot.Domain.Weather.Models;

namespace WeatherBot.Domain.Weather.Helpers;

public static class WeatherHelper
{
    public static class CountryCode
    {
        public static string Russia = "ISO 3166-2:RU";
    }

    public static string GetWeatherApiResponseString(WeatherApiResponse? weatherApiResponse)
    {
        if (weatherApiResponse == null)
            return string.Empty;

        var cityName = weatherApiResponse.Name;

        // Апи по какой-то причине определяет название Екатеринбурга и ещё некоторых городов неправильно
        if (cityName == "Posëlok Rabochiy")
            cityName = "Екатеринбург";

        return $"Погода в городе {cityName}: {weatherApiResponse.WeatherInfo.Description}, " +
               $"температура {(int) weatherApiResponse.Main.Temp}, " +
               $"ощущается как {(int) weatherApiResponse.Main.FeelsLike}";
    }
}

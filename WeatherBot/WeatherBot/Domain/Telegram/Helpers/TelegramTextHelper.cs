namespace WeatherBot.Domain.Telegram.Helpers;

public class TelegramTextHelper
{
    public static class Commands
    {
        public static string Start => "/start";
        public static string GetWeather => "/get_weather";
        public static string GetWeatherByLocation => "/get_weather_by_location";
    }
}

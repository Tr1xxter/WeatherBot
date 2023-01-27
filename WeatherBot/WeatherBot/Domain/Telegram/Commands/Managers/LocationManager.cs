using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Vostok.Logging.Abstractions;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Weather;
using WeatherBot.Domain.Weather.Helpers;

namespace WeatherBot.Domain.Telegram.Commands.Managers;

public class LocationManager
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly WeatherService _weatherService;
    private readonly ILog _log;

    public LocationManager(TelegramBotClient telegramBotClient, WeatherService weatherService, ILog log)
    {
        _telegramBotClient = telegramBotClient;
        _weatherService = weatherService;
        _log = log;
    }

    public async Task<bool> TryHandleLocation(Message message)
    {
        var location = message.Location;

        if (location == null)
        {
            _log.Error("Попытка обработать сообщение без локации.");
            return false;
        }

        var cityWeather = _weatherService.GetCityWeatherByCoordinates((float) location.Latitude, (float) location.Longitude);

        if (cityWeather == null)
        {
            _log.Error($"Не удалось получить погоду по координатам ({location.Latitude}, {location.Longitude}).");
            return false;
        }

        var text = WeatherHelper.GetWeatherApiResponseString(cityWeather);

        await _telegramBotClient.SendTextMessage(
            chatId: message.Chat.Id,
            text: text,
            parseMode: ParseMode.Markdown
        );

        return true;
    }
}

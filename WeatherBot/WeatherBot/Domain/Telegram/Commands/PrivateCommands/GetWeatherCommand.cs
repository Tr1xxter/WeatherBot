using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Helpers;
using WeatherBot.Domain.Weather;
using WeatherBot.Domain.Weather.Helpers;

namespace WeatherBot.Domain.Telegram.Commands.PrivateCommands;

public class GetWeatherCommand : IBotCommand
{
    public string Name => TelegramTextHelper.Commands.GetWeather;
    public string Help => "Сообщает текущую погоду в указанном городе России. По умолчанию город - Екатринбург";

    private readonly TelegramBotClient _telegramBotClient;
    private readonly WeatherService _weatherService;

    public GetWeatherCommand(TelegramBotClient telegramBotClient, WeatherService weatherService)
    {
        _telegramBotClient = telegramBotClient;
        _weatherService = weatherService;
    }

    public async Task ExecuteAsync(Message message, string[] args)
    {
        var cityName = "Екатеринбург";
        if (args.Length != 0) cityName = args.Aggregate((current, arg) => current + " " + arg);
        var cityWeather = _weatherService.GetCityWeather(cityName);
        var text = "Простите, мы не знаем такого города";
        if (cityWeather != default) text = WeatherHelper.GetWeatherApiResponseString(cityWeather);
        await _telegramBotClient.SendTextMessage(
            chatId: message.Chat.Id,
            text: text,
            parseMode: ParseMode.Markdown
        );
    }
}

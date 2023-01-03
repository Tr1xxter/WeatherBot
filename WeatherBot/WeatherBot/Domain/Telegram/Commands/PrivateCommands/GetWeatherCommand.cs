using System.Net.Mime;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Helpers;
using WeatherBot.Domain.Weather;
using WeatherBot.Domain.Weather.Helpers;
using WeatherBot.Domain.Weather.Models;

namespace WeatherBot.Domain.Telegram.Commands.PrivateCommands;

public class GetWeatherCommand : IBotCommand
{
    public string Name => TelegramTextHelper.Commands.GetWeather;
    public string Help => "Сообщает текущую погоду";

    private readonly TelegramBotClient _telegramBotClient;
    private readonly WeatherService _weatherService;

    public GetWeatherCommand(TelegramBotClient telegramBotClient, WeatherService weatherService)
    {
        _telegramBotClient = telegramBotClient;
        _weatherService = weatherService;
    }
    public async Task ExecuteAsync(Message message, string[] args)
    {
        var poo = _weatherService.GetCityWeather();
        var text = WeatherHelper.GetWeatherApiResponseString(poo);
        await _telegramBotClient.SendTextMessage(
            chatId: message.Chat.Id,
            text: text,
            parseMode: ParseMode.Markdown
        );
    }
}

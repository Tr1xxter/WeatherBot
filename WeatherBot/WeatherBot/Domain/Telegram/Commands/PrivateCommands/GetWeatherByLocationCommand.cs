using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Helpers;

namespace WeatherBot.Domain.Telegram.Commands.PrivateCommands;

public class GetWeatherByLocationCommand : IBotCommand
{
    public string Name => TelegramTextHelper.Commands.GetWeatherByLocation;
    public string Help => "Сообщает текущую погоду, используя вашу локацию";

    private readonly TelegramBotClient _telegramBotClient;

    public GetWeatherByLocationCommand(TelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task ExecuteAsync(Message message, string[] args)
    {
        await _telegramBotClient.SendTextMessage(
            chatId: message.Chat.Id,
            text: "Пожалуйста, отправьте свою локацию",
            parseMode: ParseMode.Markdown,
            replyMarkup: KeyboardMarkupHelper.RequestLocation
        );
    }
}

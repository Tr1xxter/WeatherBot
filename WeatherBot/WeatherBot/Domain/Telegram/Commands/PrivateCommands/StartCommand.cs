using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Helpers;

namespace WeatherBot.Domain.Telegram.Commands.PrivateCommands
{
    public class StartCommand : IBotCommand
    {
        public string Name => TelegramTextHelper.Commands.Start;
        public string Help => "Выводит приветственное сообщение";

        private readonly TelegramBotClient _telegramBotClient;

        public StartCommand(TelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Message message, string[] args)
        {
            const string text = "Здравствуйте! Я телеграм бот, позволяющий получить актуальную информацию" +
                                " о погоде в Екатеринбурге.";

            var chatId = message.Chat.Id;

            await _telegramBotClient.SendTextMessage(
                chatId: chatId,
                text: text,
                parseMode: ParseMode.Markdown
            );
        }
    }
}

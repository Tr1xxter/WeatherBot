using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Vostok.Logging.Abstractions;

namespace WeatherBot.Domain.Telegram.Clients
{
    public class TelegramBotClient
    {
        private ITelegramBotClient? _telegramBotClient;

        private const string ClientIsNullError = "Телеграм клиент не запущен!";
        private readonly ILog _log;

        public TelegramBotClient(ILog log)
        {
            _log = log;
        }

        public async Task Start(
            string botToken,
            ReceiverOptions receiverOptions,
            Func<ITelegramBotClient, Update, CancellationToken, Task> updateHandler,
            Func<ITelegramBotClient, Exception, CancellationToken, Task> pollingErrorHandler,
            CancellationToken cancellationToken)
        {
            if (_telegramBotClient != null)
                return;

            _telegramBotClient = new global::Telegram.Bot.TelegramBotClient(botToken);

            _telegramBotClient.StartReceiving(
                updateHandler: updateHandler,
                pollingErrorHandler: pollingErrorHandler,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken
            );

            var currentBot = await GetMe();

            if (currentBot == null)
                return;

            _log.Info($"Запущен клиент для телеграм бота [@{currentBot.Username}].");
        }

        public async Task<User?> GetMe()
        {
            if (_telegramBotClient == null)
            {
                _log.Error(ClientIsNullError);
                return null;
            }

            try
            {
                return await _telegramBotClient.GetMeAsync();
            }
            catch (Exception e)
            {
                _log.Error($"Не удалось получить информацию о телеграм боте. [{e.Message}]");
                return null;
            }
        }

        public async Task<Message?> SendTextMessage(
            ChatId chatId,
            string text,
            ParseMode parseMode = ParseMode.Markdown,
            IReplyMarkup? replyMarkup = null)
        {
            if (_telegramBotClient == null)
            {
                _log.Error(ClientIsNullError);
                return null;
            }

            try
            {
                return await _telegramBotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    parseMode: parseMode,
                    replyMarkup: replyMarkup);
            }
            catch (Exception e)
            {
                _log.Error($"Не получилось отправить сообщение в чат [{chatId}]. [{e.Message}]");
                return null;
            }
        }

        public async Task<IEnumerable<BotCommand>?> GetCommands()
        {
            if (_telegramBotClient == null)
            {
                _log.Error(ClientIsNullError);
                return null;
            }

            try
            {
                return await _telegramBotClient.GetMyCommandsAsync();
            }
            catch (Exception e)
            {
                _log.Error($"Не удалось получить меню команд. [{e.Message}]");
                return null;
            }
        }

        public async Task<bool> SetCommands(IEnumerable<BotCommand> botCommands)
        {
            if (_telegramBotClient == null)
            {
                _log.Error(ClientIsNullError);
                return false;
            }

            try
            {
                await _telegramBotClient.SetMyCommandsAsync(botCommands);
            }
            catch (Exception e)
            {
                _log.Error($"Не удалось задать меню команд. [{e.Message}]");
                return false;
            }

            return true;
        }
    }
}

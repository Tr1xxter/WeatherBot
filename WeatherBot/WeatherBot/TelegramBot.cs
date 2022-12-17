using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Vostok.Logging.Abstractions;
using WeatherBot.Configuration;

namespace WeatherBot
{
    public class TelegramBot
    {
        private readonly ILog _log;
        private readonly SecretsConfig _secretsConfig;

        public TelegramBot(
            ILog log,
            SecretsConfig secretsConfig)
        {
            _log = log;
            _secretsConfig = secretsConfig;
        }

        public async Task Start()
        {
#if DEBUG
            await Start(_secretsConfig.BotTestToken);
#else
            await Start(_secretsConfig.BotToken);
#endif
        }

        private async Task Start(string botToken)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message },
                ThrowPendingUpdates = true,
            };

            _log.Info("Start");

#if DEBUG
            await Task.Delay(-1, cancellationTokenSource.Token);
#endif
        }

        private async Task HandleReceivedUpdate(
            ITelegramBotClient telegramBotClient,
            Update update,
            CancellationToken cancellationToken)
        {
            var me = await telegramBotClient.GetMeAsync(cancellationToken: cancellationToken);

            switch (update.Type)
            {
                case UpdateType.Message:
                    await HandleReceivedMessage(update.Message, me);
                    return;

                default:
                    return;
            }
        }

        private async Task HandleReceivedMessage(Message message, User bot)
        {
            var messageText = message.Text;

            if (string.IsNullOrWhiteSpace(messageText))
                return;

            _log.Info($"Бот [@{bot.Username}] получил сообщение [{messageText}] в чате с [{message.From.Username}].");
        }

        private Task HandleApiError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.Message,
            };

            _log.Error(errorMessage);
            return Task.CompletedTask;
        }
    }
}

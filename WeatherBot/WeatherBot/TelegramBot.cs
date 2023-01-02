using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Vostok.Logging.Abstractions;
using WeatherBot.Configuration;
using WeatherBot.Domain.Telegram.Commands.Managers;
using TelegramBotClient = WeatherBot.Domain.Telegram.Clients.TelegramBotClient;

namespace WeatherBot
{
    public class TelegramBot
    {
        private readonly ILog _log;
        private readonly SecretsConfig _secretsConfig;
        private readonly TelegramBotClient _telegramBotClient;
        private readonly PrivateCommandManager _privateCommandManager;

        public TelegramBot(
            ILog log,
            SecretsConfig secretsConfig,
            TelegramBotClient telegramBotClient,
            PrivateCommandManager privateCommandManager)
        {
            _log = log;
            _secretsConfig = secretsConfig;
            _telegramBotClient = telegramBotClient;
            _privateCommandManager = privateCommandManager;
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

            await _telegramBotClient.Start(
                botToken,
                receiverOptions,
                HandleReceivedUpdate,
                HandleApiError,
                cancellationTokenSource.Token);

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

            await _privateCommandManager.TryPerformCommand(message);
        }

        private Task HandleApiError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error: [{apiRequestException.ErrorCode}] {apiRequestException.Message}",
                _ => exception.Message,
            };

            _log.Error(errorMessage);
            return Task.CompletedTask;
        }
    }
}

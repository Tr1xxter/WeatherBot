using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Vostok.Logging.Abstractions;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Commands.PrivateCommands;

namespace TelegramBot.Commands.OriginalTelegramBot.Managers
{
    public class PrivateCommandManager
    {
        private readonly Dictionary<string, IBotCommand> _commandNameToCommand;
        private readonly TelegramBotClient _telegramBotClient;
        private readonly ILog _log;

        public PrivateCommandManager(
            IEnumerable<IBotCommand> commands,
            TelegramBotClient telegramBotClient,
            ILog log)
        {
            _telegramBotClient = telegramBotClient;
            _log = log;

            _commandNameToCommand = new Dictionary<string, IBotCommand>(commands.ToDictionary(c => c.Name));
        }

        public async Task<bool> TryPerformCommand(Message message)
        {
            if (string.IsNullOrEmpty(message.Text?.Trim()) || message.Chat.Type is not ChatType.Private)
                return false;

            if (!TryParseCommandInformation(
                    message.Text.Trim(),
                    out var commandName,
                    out var isCommand,
                    out var args,
                    IsCommand))
            {
                return false;
            }

            if (!commandName.StartsWith("/"))
                return false;

            if (!isCommand)
            {
                await _telegramBotClient.SendTextMessage(
                    message.Chat.Id,
                    "Неизвестная команда!"
                );

                return false;
            }

            await _commandNameToCommand[commandName].ExecuteAsync(message, args);
            return true;
        }

        private static bool TryParseCommandInformation(
            string input,
            out string commandName,
            out bool isCommand,
            out string[] args,
            Func<string, bool> isCommandFunction)
        {
            var inputSplit = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (inputSplit.Length == 0)
            {
                commandName = string.Empty;
                isCommand = false;
                args = Array.Empty<string>();
                return false;
            }

            commandName = inputSplit[0];
            isCommand = isCommandFunction(commandName);
            args = inputSplit[1..];

            return true;
        }

        private bool IsCommand(string input) => _commandNameToCommand.ContainsKey(input);
    }
}

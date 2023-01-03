using Telegram.Bot.Types;

namespace WeatherBot.Domain.Telegram.Commands.PrivateCommands
{
    public interface IBotCommand
    {
        string Name { get; }
        string Help { get; }
        Task ExecuteAsync(Message message, string[] args);
    }
}

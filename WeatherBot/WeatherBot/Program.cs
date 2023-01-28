using Autofac;
using WeatherBot.DI;
using WeatherBot.Domain.Telegram.Bots;

namespace WeatherBot;

public static class Program
{
    public static async Task Main()
    {
        var container = BotContainerBuilder.Build();

        await container.Resolve<TelegramBot>().Start();
    }
}

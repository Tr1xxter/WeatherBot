using Autofac;
using WeatherBot.DI;

namespace WeatherBot;

public static class Program
{
    public static async Task Main()
    {
        var container = BotContainerBuilder.Build();

        await container.Resolve<TelegramBot>().Start();
    }
}

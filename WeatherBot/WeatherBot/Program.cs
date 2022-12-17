using Autofac;
using Vostok.Logging.Abstractions;
using WeatherBot.DI;

namespace WeatherBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var container = BotContainerBuilder.Build();

        var log = container.Resolve<ILog>();

        log.Info("Test");

        // await Task.Delay(-1);
    }
}

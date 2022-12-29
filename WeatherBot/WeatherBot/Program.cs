using Autofac;
using WeatherBot.DI;
using WeatherBot.Domain.Weather;

namespace WeatherBot;

public static class Program
{
    public static async Task Main()
    {
        var container = BotContainerBuilder.Build();

        container.Resolve<WeatherService>().GetWeather();

        await container.Resolve<TelegramBot>().Start();
    }
}

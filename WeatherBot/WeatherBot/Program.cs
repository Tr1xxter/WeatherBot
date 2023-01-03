using Autofac;
using WeatherBot.DI;
using WeatherBot.Domain.Weather;
using WeatherBot.Domain.Weather.Models;

namespace WeatherBot;

public static class Program
{
    public static async Task Main()
    {
        var container = BotContainerBuilder.Build();

        container.Resolve<WeatherService>().GetCityWeather();

        await container.Resolve<TelegramBot>().Start();
    }
}

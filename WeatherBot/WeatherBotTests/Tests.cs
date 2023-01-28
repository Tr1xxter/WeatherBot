using Autofac;
using Telegram.Bot.Polling;
using Vostok.Logging.Console;
using WeatherBot.Configuration;
using WeatherBot.DI;
using WeatherBot.Domain.Telegram.Bots;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Weather;

namespace WeatherBotTests;

public class Tests
{
    private IContainer _container;

    [SetUp]
    public void Setup()
    {
        _container = BotContainerBuilder.Build();
    }

    [Test]
    public async Task TestIfTelegramClientIsDisabledOnInitializing()
    {
        var telegramBotClient = _container.Resolve<TelegramBotClient>();
        var bot = await telegramBotClient.GetMe();

        Assert.That(bot, Is.Null);
    }

    [Test]
    public async Task TestIfTelegramBotIsEnabled()
    {
        var telegramBotClient = _container.Resolve<TelegramBotClient>();
        var secrets = _container.Resolve<SecretsConfig>();

        await telegramBotClient.Start(
            secrets.BotTestToken,
            new ReceiverOptions(),
            (_, _, _) => Task.CompletedTask,
            (_, _, _) => Task.CompletedTask,
            CancellationToken.None);

        var bot = await telegramBotClient.GetMe();

        Assert.That(bot, Is.Not.Null);
    }

    [Test]
    public void TestIfWeatherApiResponseIsNotNullWhenRequestingByCoordinates()
    {
        var weatherService = _container.Resolve<WeatherService>();
        var weatherApiResponse = weatherService.GetCityWeatherByCoordinates(0, 0);

        Assert.That(weatherApiResponse, Is.Not.Null);
    }
}

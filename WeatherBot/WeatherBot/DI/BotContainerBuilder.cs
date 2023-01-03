using System.Reflection;
using Autofac;
using Vostok.Configuration;
using Vostok.Configuration.Abstractions;
using Vostok.Configuration.Sources.Json;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;
using WeatherBot.Configuration;
using WeatherBot.Domain.Telegram.Clients;
using WeatherBot.Domain.Telegram.Commands.Managers;
using WeatherBot.Domain.Telegram.Commands.PrivateCommands;

namespace WeatherBot.DI;

public static class BotContainerBuilder
{
    public static IContainer Build()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder
            .Register<IConfigurationProvider>(cc =>
            {
                var provider = new ConfigurationProvider();
                provider.SetupSourceFor<SecretsConfig>(new JsonFileSource("Settings/secrets.json"));
                return provider;
            }).Named<IConfigurationProvider>(ConfigurationScopes.BotSettingsScope);

        containerBuilder.Register(cc => cc
            .ResolveNamed<IConfigurationProvider>(ConfigurationScopes.BotSettingsScope)
            .Get<SecretsConfig>());

        var currentAssembly = Assembly.GetExecutingAssembly();

        containerBuilder.RegisterAssemblyTypes(currentAssembly).As<IBotCommand>();

        containerBuilder.RegisterType<PrivateCommandManager>();

        containerBuilder.RegisterType<TelegramBot>().SingleInstance();
        containerBuilder.RegisterType<TelegramBotClient>().SingleInstance();

        containerBuilder.Register<ILog>(cc =>
        {
            var consoleLog = new ConsoleLog();
            var fileLogSettings = new FileLogSettings
            {
                RollingStrategy = new RollingStrategyOptions
                {
                    MaxFiles = 10,
                    MaxSize = 1024 * 1024 * 100,
                    Period = RollingPeriod.Day,
                    Type = RollingStrategyType.Hybrid
                }
            };
            var fileLog = new FileLog(fileLogSettings);
            return new CompositeLog(consoleLog, fileLog);
        }).SingleInstance();

        return containerBuilder.Build();
    }
}

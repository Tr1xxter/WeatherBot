using Autofac;

namespace WeatherBot.DI;

public static class BotContainerBuilder
{
    public static IContainer Build()
    {
        var containerBuilder = new ContainerBuilder();

        return containerBuilder.Build();
    }
}

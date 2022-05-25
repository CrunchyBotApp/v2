using CrunchyBotNext.Bot;
using CrunchyBotNext.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Sentry;

namespace CrunchyBotNext;

public class Program
{
    public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

    private static async Task MainAsync(string[] args)
    {
        ServiceProvider services = new ServiceCollection()
            .AddSingleton(new DiscordSocketConfig()
            {
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.AllUnprivileged
            })
            .AddSingleton<CommandService>()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandHandlerService>()
            .AddSingleton<LoggerService>()
            .AddSingleton<BaseService>()
            .BuildServiceProvider();

        CrunchyBot<DiscordSocketClient> bot = new(await JsonConfigurationManager.GetConfiguration() ?? new Configuration(), services);

        await bot.BootAsync();

        await Task.Delay(-1);
    }
}
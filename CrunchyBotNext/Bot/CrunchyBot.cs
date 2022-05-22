using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using Serilog;

namespace CrunchyBotNext.Bot
{
    public class CrunchyBot<TClient> where TClient : BaseSocketClient // make it so you can hotswap TClient with a DiscordSocketClient or a DiscordShardedClient
    {
        private readonly Configuration _config;
        private readonly ServiceProvider _services;

        public CrunchyBot(Configuration config, ServiceProvider services)
        {
            _config = config;
            _services = services;
        }

        public CrunchyBot(string token, string prefix, ServiceProvider services, string? sentryDsn = null)
        {
            _config = new Configuration()
            {
                DiscordToken = token,
                Prefix = prefix,
                SentryDsn = sentryDsn
            };
            _services = services;
        }

        public async Task BootAsync()
        {
            TClient client = _services.GetRequiredService<TClient>();

            Log.Information("[BootAsync] BUILDIN' THE SENTRY!");

            SentrySdk.Init(c =>
            {
                c.Dsn = _config.SentryDsn;
                c.Debug = true;
                c.TracesSampleRate = 1.0d; // TODO: reduce this value in prod
            });

            await client.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await client.StartAsync();
        }
    }
}

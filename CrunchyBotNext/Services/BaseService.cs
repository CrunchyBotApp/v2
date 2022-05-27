using Discord;
using Discord.WebSocket;
using Sentry;

namespace CrunchyBotNext.Services
{
    public class BaseService
    {
        private readonly DiscordSocketClient _client;
        private Timer _timer;
        private int _statusIndex = 0;
        private List<Game> statuses;

        public BaseService(DiscordSocketClient client)
        {
            _client = client;
        }

        public void Initialise()
        {
            statuses = new()
            {
                new Game("c-help", ActivityType.Listening),
                new Game($"with images"),
                new Game($"CrunchCon {DateTime.Now.Year}", ActivityType.Competing),
                new Game("do not ingest lethal chemicals"),
                new Game("crunched up !"),
                new Game("its about time that i mine the diamond"),
                new Game("march 28th 2010"),
                new Game("rip annotations"),
                new Game("milk"),
                new Game("rule 1. do not talk about dentist role"),
                new Game("This man clearly doesn't want to draw them being eaten 😱😱😱"),
                new Game("we do not talk about the 5 cents")
            };

            _client.Ready += async () =>
            {
                _timer = new Timer(async _ =>
                    {    
                        await _client.SetActivityAsync(statuses.ElementAtOrDefault(_statusIndex));
                        _statusIndex = _statusIndex + 1 == statuses.Count ? 0 : _statusIndex + 1;
                    },
                    null,
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(15));
            };

            _client.JoinedGuild += async (guild) =>
            {
                await guild.DefaultChannel.SendFileAsync("OtherAssets/Hewllo.mp4", "thank you for inviting crunchybot\nrun c-help for a list of commands and remember to PAY YOUR TAXE-");
            };
        }
    }
}

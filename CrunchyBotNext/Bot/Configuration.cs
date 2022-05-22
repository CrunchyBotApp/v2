using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchyBotNext.Bot
{
    public class Configuration
    {
        public string DiscordToken { get; init; }
        public string Prefix { get; init; }
        public string? SentryDsn { get; init; }
    }
}

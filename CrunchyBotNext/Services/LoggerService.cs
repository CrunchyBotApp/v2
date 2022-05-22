using Discord;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.File;

namespace CrunchyBotNext.Services
{
    public class LoggerService
    {
        private readonly DiscordSocketClient _client;

        public LoggerService(DiscordSocketClient client)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            _client.Log += LogAsync;
        }

        private async Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Debug => LogEventLevel.Debug,
                _ => LogEventLevel.Information
            };

            Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);
            await Task.CompletedTask;
        }
    }
}

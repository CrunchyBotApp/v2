# CrunchyBot
Invite the bot: [here](https://is.gd/CrunchyBot) \
I won't even go over any of the crap nobody cares about, just straight to a setup guide.

# Setup
To setup CrunchyBot, clone this repository, and make sure you have the .NET 6.0 runtime installed. \
Then, create a file named CrunchyConfig.json inside the CrunchyBot folder. Populate it with this data:
```json
{
  "DiscordToken": "token-here",
  "Prefix": "c-",
  "SentryDsn": "",
  "TenorAPIKey": "key-here"
}
```
Leave SentryDsn blank if you don't want to setup Sentry. All other configuration options are required. \
Once you've got the configuration set up, run the bot using `dotnet run`. \
That's all there is to it.

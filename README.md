# CrunchyBot
Invite the bot: [here](https://is.gd/CrunchyBot) \
If you want to set up and run the bot on your own hardware, continue to the Setup section. \
Otherwise, if you want to contribute to CrunchyBot's code, continue to the Contributing section.

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

# Contributing
Whether you want to make a new feature, fix a bug, or anything, you can do all that with a pull request. I appreciate your support, and I will try to review and merge every pull request if it looks good to me.

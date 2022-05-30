using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrunchyBotNext.Bot;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Sentry;

namespace CrunchyBotNext.Utilities
{
    public static class ImageDetection
    {
        private static string[] tenorURLs = {
        "tenor.com",
        "www.tenor.com"
        };
        private static string[] giphyURLs = {
        "giphy.com",
        "www.giphy.com",
        "i.giphy.com"
        };
        private static string[] giphyMediaURLs = {
        "media.giphy.com",
        "media0.giphy.com",
        "media1.giphy.com",
        "media2.giphy.com",
        "media3.giphy.com",
        "media4.giphy.com"
        };

        public static async Task<string> GetImage(string imageUri)
        {
            using var httpHandler = new SentryHttpMessageHandler();
            using var httpClient = new HttpClient(httpHandler);

            var host = new Uri(imageUri).Host;

            if (tenorURLs.Contains(host))
            {
                string req =
                    $"https://g.tenor.com/v1/gifs?ids={imageUri.Split("-").Last()}&media_filter=minimal&limit=1&key={(await JsonConfigurationManager.GetConfiguration()).TenorAPIKey}";
                string data = await httpClient.GetStringAsync(req);
                Console.WriteLine(req);


                TenorData json = JsonConvert.DeserializeObject<TenorData>(data);

                return json.results[0].media[0].gif.url;
            }
            else if (giphyURLs.Contains(host)) {
                return $"https://media0.giphy.com/media/{imageUri.Split("/").Last().Split('-').Last()}/giphy.gif";
            }
            else if (giphyMediaURLs.Contains(host)) {
                return $"https://media0.giphy.com/media/{imageUri.Split("/").Last().Split('-').Last()}/giphy.gif";
            }
            else
            {
                return imageUri;
            }

            return imageUri;
        }

        public static async Task<Uri?> Detect(SocketCommandContext Context)
        {
            var messages = await Context.Channel
                .GetMessagesAsync(Context.Message, Direction.Before)
                .FlattenAsync();

            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            try
            {
                if (Context.Message.Attachments.Any())
                {
                    return new Uri(Context.Message.Attachments.ToList()[0].ProxyUrl);
                }

                var message =
                    messages.First(
                        m => 
                            Rgx.IsMatch(m.Content)
                            || m.Attachments.Any()
                            );

                Uri? uri;

                _ = (Uri.TryCreate(message.Content, UriKind.Absolute, out uri))
                    ? uri = new Uri(
                        await ImageDetection.GetImage(message.Content)) // gets proper tenor or giphy urls for downloading
                    : (Context.Message.Attachments.Any())
                        ? uri = new Uri(Context.Message.Attachments.ToList()[0].ProxyUrl)
                        : (message.Attachments.Any())
                                ? uri = new Uri(message.Attachments.ToList()[0].ProxyUrl)
                                : uri = null;

                return uri;
            }
            catch (ArgumentNullException e)
            {
                throw new Exception("No images have been found within the last 100 messages. Please try sending an image and then run the command again.");
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

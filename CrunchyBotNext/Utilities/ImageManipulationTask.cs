using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;
using Sentry;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using Serilog;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CrunchyBotNext.Utilities
{
    public class ImageManipulationTask
    {
        private readonly SocketCommandContext _ctx;
        private Image<Rgba32> _image;
        private readonly Func<Image<Rgba32>, Image<Rgba32>> _processingFunc;

        public ImageManipulationTask(SocketCommandContext ctx, Func<Image<Rgba32>, Image<Rgba32>> processingFunc)
        {
            _ctx = ctx;
            _processingFunc = processingFunc;
        }

        public async Task Start(string processingMessage = "processing", bool isLongOperation = false)
        {
            var message = await _ctx.Channel.SendMessageAsync(processingMessage);

            Uri? detected = await ImageDetection.Detect(_ctx);
            using var httpHandler = new SentryHttpMessageHandler();
            using var httpClient = new HttpClient(httpHandler);

            Stream input = await httpClient.GetStreamAsync(detected);
            Image<Rgba32> image = await Image.LoadAsync<Rgba32>(input);

            var i = _processingFunc.Invoke(image);

            Stream output = new MemoryStream();

            await i.SaveAsync(output, image.DetectEncoder(detected.OriginalString));
            await message.ModifyAsync(x => x.Content = "ready");
            await _ctx.Channel.SendFileAsync(output, Path.GetFileName(detected.OriginalString));
        }
    }

    public class GifImageManipulationTask
    {
        private readonly SocketCommandContext _ctx;
        private readonly Func<Image, Image> _processingFunc;

        public GifImageManipulationTask(SocketCommandContext ctx, Func<Image, Image> processingFunc)
        {
            _ctx = ctx;
            _processingFunc = processingFunc;
        }

        public async Task Start(string processingMessage = "processing", bool isLongOperation = false)
        {
            var message = await _ctx.Channel.SendMessageAsync(processingMessage);

            Uri? detected = await ImageDetection.Detect(_ctx);
            var httpHandler = new SentryHttpMessageHandler();
            var httpClient = new HttpClient(httpHandler);

            Stream input = await httpClient.GetStreamAsync(detected);
            Image image = await Image.LoadAsync(input);

            var i = _processingFunc.Invoke(image);

            Stream output = new MemoryStream();

            Log.Information("saving");
            await i.SaveAsGifAsync(output);
            Log.Information("saved");

            await message.ModifyAsync(x => x.Content = "ready");
            await _ctx.Channel.SendFileAsync(output, "giffed.gif");
        }
    }
}

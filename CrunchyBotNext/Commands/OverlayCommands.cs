using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrunchyBotNext.Utilities;
using Discord.Commands;
using Sentry;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CrunchyBotNext.Commands
{
    [Summary("Overlays")]
    public class OverlayCommands : ModuleBase<SocketCommandContext>
    {
        [Command("gayify")]
        [Summary("I think it should be pretty obvious what this command does")]
        [Alias("gay", "pride", "prideflag", "gayflag", "rainbowflag")]
        public async Task Gayify(int factor = 1, bool trueCrunch = true)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                Image over = Image.Load("OverlayImages/g.png");
                var task = new ImageManipulationTask(Context, i =>
                {
                    over.Mutate(x=>x.Resize(i.Width, i.Height));
                    i.Mutate(x =>
                    {
                        x.DrawImage(over, 0.5f);
                    });

                    return i;
                });

                await task.Start("keep in mind that the owner of crunchybot is accepting of everyone and is not homophobic; someone just really wanted him to add this in");
            });
        }

        [Command("eaten")]
        [Summary("This man clearly doesn't want to draw them being eaten 😱😱😱")]
        public async Task Eaten()
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                Image<Rgba32> over = Image.Load<Rgba32>("OverlayImages/eaten.png");
                var task = new ImageManipulationTask(Context, i =>
                {
                    i.Mutate(x=>x.Resize(160, 108));
                    over.Mutate(x =>
                    {
                        x.DrawImage(i, new Point(85, 133), 1f);
                    });

                    return over;
                });

                await task.Start("😱");
            });
        }
    }
}

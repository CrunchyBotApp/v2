using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CrunchyBotNext.Utilities;
using Discord;
using Discord.Commands;
using Sentry;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;

namespace CrunchyBotNext.Commands
{
    [Summary("Fun")]
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        [Command("runch", RunMode = RunMode.Async)]
        [Alias("crunch")]
        [Summary("So uhh you gone crunch? Now you can express your inner crunchiness by Crunching images here")]
        public async Task Crunch(int factor = 1)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                var task = new ImageManipulationTask(Context, i =>
                {
                    i.Mutate(x =>
                    {
                        x.Resize(i.Width / (1 + factor), i.Height / (1 + factor));
                        x.Resize(i.Width * (1 + factor), i.Height * (1 + factor));
                    });

                    return i;
                });

                await task.Start("CRUUUUUUUUUUUUUUUUUUUUUUUUNCH !");
            });
        }

        [Command("deepfry", RunMode = RunMode.Async)]
        [Alias("oversaturate", "fries", "imagestove")]
        [Summary("microwave.mp3 the 2nd")]
        public async Task Deepfry(float factor = 1)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                var task = new ImageManipulationTask(Context, i =>
                {
                    i.Mutate(x =>
                    {
                        x.Saturate(32 * factor);
                    });

                    return i;
                });

                await task.Start();
            });
        }

        [Command("shake", RunMode = RunMode.Async)]
        [Alias("angy", "angry")]
        [Summary("shake algos are too complicated so we just use Random")]
        public async Task Shake(int intensity = 1)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                var task = new GifImageManipulationTask(Context, i =>
                {
                    Image<Rgba32> animated = new(i.Width, i.Height);

                    animated.Mutate(x => {
                        x.DrawImage(i, 1f);
                    });


                    var gifMetaData = animated.Metadata.GetGifMetadata();
                    gifMetaData.RepeatCount = ushort.MaxValue;

                    GifFrameMetadata metadata = animated.Frames.RootFrame.Metadata.GetGifMetadata();
                    metadata.FrameDelay = 0;

                    for (int j = 0; j < 180; j += 2)
                    {
                        using Image<Rgba32> image = new(i.Width, i.Height, Color.Black);

                        image.Mutate(x => {
                            x.DrawImage(i, new Point(
                                Random.Shared.Next(-4 * intensity, 4 * intensity),
                                Random.Shared.Next(-4 * intensity, 4 * intensity)), 1f);
                        });
                        metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
                        metadata.FrameDelay = 2;

                        Log.Verbose("frame {Frame}", j);
                        animated.Frames.AddFrame(image.Frames.RootFrame);
                    }

                    Log.Debug("operation complete");

                    i = animated;

                    return i;
                });

                await task.Start(isLongOperation: true);
            });
        }

        [Command("gamer", RunMode = RunMode.Async)]
        [Alias("rgb", "gaming", "rainbowlights", "rainbow")]
        [Summary("are you a gamer? this command is for you! unleash your rgb addiction today")]
        public async Task Gamer(float speed = 1f)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                var task = new GifImageManipulationTask(Context, i =>
                {
                    Image<Rgba32> animated = new(i.Width, i.Height);

                    animated.Mutate(x => {
                        x.DrawImage(i, 1f);
                    });


                    var gifMetaData = animated.Metadata.GetGifMetadata();
                    gifMetaData.RepeatCount = ushort.MaxValue;

                    GifFrameMetadata metadata = animated.Frames.RootFrame.Metadata.GetGifMetadata();
                    metadata.FrameDelay = 0;

                    for (int j = 0; j < 180; j += 2)
                    {
                        using Image<Rgba32> image = new(i.Width, i.Height);

                        image.Mutate(x => {
                            x.DrawImage(i, 1f);
                            x.Hue(j * 2);
                        });
                        metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
                        metadata.FrameDelay = 100 / (int)(speed * 10);

                        Log.Verbose("frame {Frame}", j);
                        animated.Frames.AddFrame(image.Frames.RootFrame);
                    }

                    Log.Debug("operation complete");

                    i = animated;

                    return i;
                });

                await task.Start(isLongOperation: true);
            });
        }

        [Command("deepfrya")]
        [Summary("deepfry but it shows you the process in gif form")]
        public async Task DeepfryAnimated(int factor = 1, int speed = 1)
        {
            await Wrappers.ExceptionCapturingWrapper(async () =>
            {
                var task = new GifImageManipulationTask(Context, i =>
                {
                    Image<Rgba32> animated = new(i.Width, i.Height);

                    animated.Mutate(x => {
                        x.DrawImage(i, 1f);
                    });


                    var gifMetaData = animated.Metadata.GetGifMetadata();
                    gifMetaData.RepeatCount = ushort.MaxValue;

                    GifFrameMetadata metadata = animated.Frames.RootFrame.Metadata.GetGifMetadata();
                    metadata.FrameDelay = 0;

                    for (int j = 0; j < 180; j += 2)
                    {
                        using Image<Rgba32> image = new(i.Width, i.Height);

                        image.Mutate(x => {
                            x.DrawImage(i, 1f);
                            x.Saturate(j * factor);
                        });
                        metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
                        metadata.FrameDelay = 100 / (speed * 10);

                        Log.Verbose("frame {Frame}", j);
                        animated.Frames.AddFrame(image.Frames.RootFrame);
                    }

                    Log.Debug("operation complete");

                    i = animated;

                    return i;
                });

                await task.Start(isLongOperation: true);
            });
        }
    }
}

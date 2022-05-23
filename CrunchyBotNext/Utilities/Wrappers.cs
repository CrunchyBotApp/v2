using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Sentry;
using Serilog;

namespace CrunchyBotNext.Utilities
{
    public class Wrappers
    {
        public delegate Task PassthroughDelegate();

        public async static Task ExceptionCapturingWrapper(PassthroughDelegate f)
        {
            try
            {
                await f.Invoke();
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);

                Log.Error(e, "oops");
                throw;
            }
        }
    }
}

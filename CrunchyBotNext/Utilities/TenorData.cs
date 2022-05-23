using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchyBotNext.Utilities
{
    public class TenorData
    {
        public TenorResult[] results { get; set; }
    }

    public class TenorResult
    {
        public TenorMedia[] media { get; set; }
    }

    public class TenorMedia
    {
        public TenorGif gif { get; set; }
    }

    public class TenorGif
    {
        public string url { get; set; }
    }
}

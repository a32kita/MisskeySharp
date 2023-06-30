using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming
{
    [Flags]
    public enum MisskeyStreamingChannels
    {
        GlobalTimeline   = 0b00000001,

        HomeTimeline     = 0b00000010,

        HybridTimeline   = 0b00000100,

        LocalTimeline    = 0b00001000,

        Main             = 0b00010000,
    }
}

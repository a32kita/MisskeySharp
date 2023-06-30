using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming.Internal2
{
    internal enum WebSocketClientState
    {
        Unknown,
        Disconnected,
        Connecting,
        Connected,
    }
}

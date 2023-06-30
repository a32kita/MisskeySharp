using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming.Internal2
{
    public class WebSocketReceivedEventArgs
    {
        public string Data
        {
            get;
            private set;
        }


        public WebSocketReceivedEventArgs(string data)
        {
            this.Data = data;
        }
    }
}

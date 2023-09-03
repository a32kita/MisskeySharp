using MisskeySharp.Streaming.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming
{
    public class MisskeyNotificationReceivedEventArgs
    {
        public NotificationMessage NotificationMessage { get; set; }
    }
}

using MisskeySharp.Streaming.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming
{
    public class MisskeyNoteReceivedEventArgs
    {
        public NoteMessage NoteMessage
        {
            get;
            set;
        }
    }
}

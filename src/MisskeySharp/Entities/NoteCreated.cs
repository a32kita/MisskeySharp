using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class NoteCreated : MisskeyApiEntitiesBase
    {
        public Note CreatedNote
        {
            get;
            set;
        }
    }
}

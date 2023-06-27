using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class Notification : MisskeyApiEntitiesBase
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public Note Note { get; set; }

        public string Reaction { get; set; }
    }
}

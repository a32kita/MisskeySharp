using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class Reactions
    {
        [JsonPropertyName("❤")]
        public int Heart { get; set; }

        [JsonPropertyName("🎉")]
        public int Tada { get; set; }

        [JsonPropertyName(":igyo@.:")]
        public int Igyo { get; set; }

        [JsonPropertyName(":moutasukaranaizo@.:")]
        public int Moutasukaranaizo { get; set; }
    }
}

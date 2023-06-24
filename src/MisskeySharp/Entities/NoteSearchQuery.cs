using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class NoteSearchQuery : MisskeyApiEntitiesBase
    {
        public string Query
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SinceId
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string UntilId
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Limit
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Offset
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Host
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string UserId
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ChannelId
        {
            get;
            set;
        }
    }
}

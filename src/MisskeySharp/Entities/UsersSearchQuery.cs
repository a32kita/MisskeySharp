using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class UsersSearchQuery : MisskeyApiEntitiesBase
    {
        public string Query
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
        public int Limit
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Origin
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Detail
        {
            get;
            set;
        }
    }
}

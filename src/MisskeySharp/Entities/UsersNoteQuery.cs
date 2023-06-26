using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class UsersNoteQuery : MisskeyApiEntitiesBase
    {
        public string UserId
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IncludeReplies
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
    }
}

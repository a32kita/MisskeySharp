using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class DriveFilesQuery : MisskeyApiEntitiesBase
    {
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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string FolderId
        {
            get;
            set;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type
        {
            get;
            set;
        }
    }
}

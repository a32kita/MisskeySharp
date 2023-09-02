using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using MisskeySharp.Entities;

namespace MisskeySharp
{
    public class MisskeyApiEntitiesBase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string I
        {
            get; set;
        }

        [JsonIgnore]
        public int HttpStatusCode
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool IsSuccess
        {
            get => this.HttpStatusCode / 100 == 2 && this.Error == null;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MisskeyApiError Error
        {
            get;
            set;
        }

        [JsonIgnore]
        public Exception JsonDeserializeError
        {
            get;
            set;
        }
    }
}

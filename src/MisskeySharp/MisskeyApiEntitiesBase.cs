using System;
using System.Collections.Generic;
using System.Text;

using MisskeySharp.Entities;

namespace MisskeySharp
{
    public class MisskeyApiEntitiesBase
    {
        public string I
        {
            get; set;
        }

        public int HttpStatusCode
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get => this.HttpStatusCode / 100 == 2 && this.Error == null;
        }

        public MisskeyApiError Error
        {
            get;
            set;
        }
    }
}

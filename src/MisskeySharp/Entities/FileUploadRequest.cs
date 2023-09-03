using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MisskeySharp.Entities
{
    public class FileUploadRequest : MisskeyApiEntitiesBase
    {
        public string FileName
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public Stream ContentStream
        {
            get;
            set;
        }
    }
}

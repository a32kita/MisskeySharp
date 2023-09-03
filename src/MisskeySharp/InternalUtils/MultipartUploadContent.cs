using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.InternalUtils
{
    public class MultipartUploadContent
    {
        public string Name
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string StringContent
        {
            get;
            set;
        }

        public byte[] ByteContent
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }
    }
}

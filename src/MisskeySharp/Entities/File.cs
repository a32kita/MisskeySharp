using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class File : MisskeyApiEntitiesBase
    {
        public string Id
        {
            get;
            set;
        }

        public DateTime CreatedAt
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string Md5
        {
            get;
            set;
        }

        public uint Size
        {
            get;
            set;
        }

        public bool IsSensitive
        {
            get;
            set;
        }

        public string Blurhash
        {
            get;
            set;
        }

        public Uri Url
        {
            get;
            set;
        }

        public Uri ThumbnailUrl
        {
            get;
            set;
        }
    }
}

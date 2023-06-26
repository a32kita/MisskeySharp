using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class MisskeyApiError
    {
        public string Message
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string Kind
        {
            get;
            set;
        }
    }
}

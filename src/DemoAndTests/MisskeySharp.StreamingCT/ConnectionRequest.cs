using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.StreamingCT
{
    public class ConnectionRequest
    {
        public string Type
        {
            get;
            set;
        }

        public BodyObject Body
        {
            get;
            set;
        }

        public class BodyObject
        {
            public string Channel
            {
                get;
                set;
            }

            public string Id
            {
                get;
                set;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming.Entities
{
    public class ConnectRequestParameter
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

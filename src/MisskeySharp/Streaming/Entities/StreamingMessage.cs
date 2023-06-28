using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Streaming.Entities
{
    public class StreamingMessage<TBody>
    {
        public string Channel
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
            public string Id
            {
                get;
                set;
            }

            public string Type
            {
                get;
                set;
            }

            public TBody Body
            {
                get;
                set;
            }
        }
    }
}

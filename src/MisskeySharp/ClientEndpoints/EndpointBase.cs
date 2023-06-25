using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.ClientEndpoints
{
    public class EndpointBase
    {
        protected MisskeyService Parent
        {
            get;
            private set;
        }


        internal EndpointBase(MisskeyService parent)
        {
            Parent = parent;
        }
    }
}

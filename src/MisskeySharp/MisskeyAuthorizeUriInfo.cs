using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp
{
    public class MisskeyAuthorizeUriInfo
    {
        public string SessionId
        {
            get;
            private set;
        }

        public Uri Uri
        {
            get;
            private set;
        }


        internal MisskeyAuthorizeUriInfo(string sessionId, Uri uri)
        {
            this.SessionId = sessionId;
            this.Uri = uri;
        }
    }
}

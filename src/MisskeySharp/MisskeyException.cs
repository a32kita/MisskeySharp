using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

using MisskeySharp.Entities;

namespace MisskeySharp
{
    public class MisskeyException : Exception
    {
        public MisskeyApiError ApiError
        {
            get;
            set;
        }


        public MisskeyException()
        {
        }

        public MisskeyException(string message) : base(message)
        {
        }

        public MisskeyException(MisskeyApiError apiError)
            : this(apiError.Message)
        {
            this.ApiError = apiError;
        }

        public MisskeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MisskeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

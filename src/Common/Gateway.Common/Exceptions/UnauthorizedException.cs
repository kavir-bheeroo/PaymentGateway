using System;
using System.Runtime.Serialization;

namespace Gateway.Common.Exceptions
{
    [Serializable]
    public class UnauthorizedException : ExceptionBase
    {
        public UnauthorizedException(string customMessage) : base(customMessage)
        {
        }

        public UnauthorizedException(
            string customMessage,
            Exception innerException) : base(customMessage, innerException)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
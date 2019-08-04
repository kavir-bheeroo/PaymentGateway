using System;
using System.Runtime.Serialization;

namespace Gateway.Common.Exceptions
{
    [Serializable]
    public class GatewayException : ExceptionBase
    {
        public GatewayException(
            string customMessage) : base(customMessage)
        {
        }

        public GatewayException(
            string customMessage,
            Exception innerException) : base(customMessage, innerException)
        {
        }

        protected GatewayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
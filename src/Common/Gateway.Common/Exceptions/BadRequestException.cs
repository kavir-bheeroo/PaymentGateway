using System;
using System.Runtime.Serialization;

namespace Gateway.Common.Exceptions
{
    [Serializable]
    public class BadRequestException : ExceptionBase
    {
        public BadRequestException(
            string customMessage) : base(customMessage)
        {
        }

        public BadRequestException(
            string customMessage,
            Exception innerException) : base(customMessage, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace Gateway.Common.Exceptions
{
    [Serializable]
    public class ObjectAlreadyExistsException : ExceptionBase
    {
        public ObjectAlreadyExistsException(string customMessage) : base(customMessage)
        {
        }

        protected ObjectAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
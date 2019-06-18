using System;
using System.Runtime.Serialization;


namespace ProximaX.Sirius.Chain.Sdk.Model.Exceptions
{
    public class TypeNotSupportException : Exception
    {
        public TypeNotSupportException()
        {
        }

        public TypeNotSupportException(string message)
            : base(message)
        {
        }

        public TypeNotSupportException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TypeNotSupportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

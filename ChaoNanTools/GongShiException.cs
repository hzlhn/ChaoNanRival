using System;
using System.Runtime.Serialization;

namespace ChaoNanTools
{
    [Serializable]
    internal class GongShiException : Exception
    {
        public GongShiException()
        {
        }

        public GongShiException(string message) : base(message)
        {
        }

        public GongShiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GongShiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
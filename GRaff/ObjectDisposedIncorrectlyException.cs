using System;
using System.Runtime.Serialization;

namespace GRaff
{
    [Serializable]
    internal class ObjectDisposedIncorrectlyException : Exception
    {
        public ObjectDisposedIncorrectlyException()
        {
        }

        public ObjectDisposedIncorrectlyException(string message) : base(message)
        {
        }

        public ObjectDisposedIncorrectlyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectDisposedIncorrectlyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
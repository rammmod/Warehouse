using System.Runtime.Serialization;

namespace Warehouse.Infrastructure.Exceptions
{
    [Serializable]
    internal class DeleteNotAcknowledgedException : Exception
    {
        public DeleteNotAcknowledgedException()
        {
        }

        public DeleteNotAcknowledgedException(string message) : base(message)
        {
        }

        public DeleteNotAcknowledgedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeleteNotAcknowledgedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message => "Delete is not acknowledged";
    }
}
using System.Runtime.Serialization;

namespace Warehouse.Infrastructure.Exceptions
{
    [Serializable]
    internal class UpdateNotAcknowledgedException : Exception
    {
        public UpdateNotAcknowledgedException()
        {
        }

        public UpdateNotAcknowledgedException(string message) : base(message)
        {
        }

        public UpdateNotAcknowledgedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UpdateNotAcknowledgedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message => "Update is not acknowledged";
    }
}
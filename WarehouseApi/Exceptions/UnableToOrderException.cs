using WarehouseApi.Exceptions.Base;

namespace WarehouseApi.Exceptions
{
    [Serializable]
    public class UnableToOrderException : HttpResponseException
    {
        public UnableToOrderException() : base(StatusCodes.Status400BadRequest, null)
        {
        }

        public UnableToOrderException(string message) : base(StatusCodes.Status400BadRequest, message)
        {
        }
        public override string Message => "Unable to make an order";
    }
}
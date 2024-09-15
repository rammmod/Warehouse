using WarehouseApi.Exceptions.Base;

namespace WarehouseApi.Exceptions
{
    [Serializable]
    public class ProductNotFoundException : HttpResponseException
    {
        public ProductNotFoundException() : base(StatusCodes.Status404NotFound, null)
        {
        }

        public ProductNotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
        {
        }

        public override string Message => "Product does not exist";
    }
}
using WarehouseApi.Exceptions.Base;

namespace WarehouseApi.Exceptions
{
    [Serializable]
    public class CategoryNotEmptyException : HttpResponseException
    {
        public CategoryNotEmptyException() : base(StatusCodes.Status400BadRequest, null)
        {
        }

        public CategoryNotEmptyException(string message) : base(StatusCodes.Status400BadRequest, message)
        {
        }

        public override string Message => "Category is not empty";
    }
}
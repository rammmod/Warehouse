using WarehouseApi.Exceptions.Base;

namespace WarehouseApi.Exceptions
{
    [Serializable]
    public class CategoryNotFoundException : HttpResponseException
    {
        public CategoryNotFoundException() : base(StatusCodes.Status404NotFound)
        {
        }

        public CategoryNotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
        {
        }

        public override string Message => "Category does not exist";
    }
}
namespace WarehouseApi.Models.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}

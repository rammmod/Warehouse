namespace WarehouseApi.Models.Category
{
    public class UpdateCategoryStockRequest
    {
        public int Id { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}

namespace WarehouseApi.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}

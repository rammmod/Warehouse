namespace WarehouseApi.Models.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}

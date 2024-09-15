namespace WarehouseApi.Domain.Entities
{
    public class Category : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}
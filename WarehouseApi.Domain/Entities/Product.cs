namespace WarehouseApi.Domain.Entities
{
    public class Product : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
    }
}
using WarehouseApi.Domain.Enums;

namespace WarehouseApi.Domain.Entities
{
    public class Order : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}

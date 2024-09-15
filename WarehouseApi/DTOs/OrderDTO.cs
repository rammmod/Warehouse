using WarehouseApi.Domain.Enums;

namespace WarehouseApi.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}

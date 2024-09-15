using WarehouseApi.Domain.Entities;
using WarehouseApi.Enums;
using MassTransit;

namespace WarehouseApi.Contracts
{
    public class CreateOrderContract : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Order Order { get; set; }
        public OutOfStockModeEnum Mode { get; set; }
    }
}

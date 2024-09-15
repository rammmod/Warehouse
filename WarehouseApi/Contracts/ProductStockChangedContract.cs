using MassTransit;

namespace WarehouseApi.Contracts
{
    public class ProductStockChangedContract : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int ProductId { get; set; }
    }
}

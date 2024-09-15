using MassTransit;

namespace WarehouseApi.Sagas.Contracts
{
    public class CreateOrderSagaCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

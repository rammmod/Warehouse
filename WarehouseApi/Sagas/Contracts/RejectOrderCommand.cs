using MassTransit;

namespace WarehouseApi.Sagas.Contracts
{
    public class RejectOrderCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
    }
}
